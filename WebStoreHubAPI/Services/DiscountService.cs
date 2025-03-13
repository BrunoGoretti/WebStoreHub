using OfficeOpenXml;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Data;
using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Dtos;
using WebStoreHubAPI.Services.Interfaces;

public class DiscountService : IDiscountService
{
    private readonly AppDbContext _dbContext;

    public DiscountService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<DiscountDto>> ImportDiscountsFromExcelAsync(Stream excelStream)
    {
        var discounts = new List<DiscountDto>();

        using (var package = new ExcelPackage(excelStream))
        {
            var worksheet = package.Workbook.Worksheets.First();
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var discountDto = new DiscountDto
                {
                    ProductId = int.Parse(worksheet.Cells[row, 1].Text),
                    DiscountPercentage = decimal.TryParse(worksheet.Cells[row, 2].Text, out var percentage) ? percentage : 0,
                    StartDate = DateTime.Parse(worksheet.Cells[row, 3].Text), 
                    EndDate = DateTime.Parse(worksheet.Cells[row, 4].Text), 
                    IsActive = bool.Parse(worksheet.Cells[row, 5].Text) 
                };
                discounts.Add(discountDto);
            }
        }

        return discounts;
    }

    public async Task ApplyDiscountsAsync(IEnumerable<DiscountDto> discounts)
    {
        foreach (var discount in discounts)
        {
            var product = await _dbContext.DbProducts.FirstOrDefaultAsync(p => p.ProductId == discount.ProductId);

            if (product != null)
            {
                decimal newDiscountedPrice = product.Price; 

                if (discount.IsActive && discount.EndDate >= DateTime.Now)
                {
                    if (discount.DiscountPercentage > 0)
                    {
                        var discountAmount = product.Price * (discount.DiscountPercentage / 100);
                        newDiscountedPrice -= discountAmount;
                    }

                    if (newDiscountedPrice < 0) newDiscountedPrice = 0;

                    product.DiscountedPrice = newDiscountedPrice;
                }
                else
                {
                    product.DiscountedPrice = null;
                }

                _dbContext.DbProducts.Update(product);

                var existingDiscount = await _dbContext.Discounts
                    .FirstOrDefaultAsync(d => d.ProductId == discount.ProductId
                                              && d.StartDate == discount.StartDate
                                              && d.EndDate == discount.EndDate);

                if (existingDiscount != null)
                {
                    existingDiscount.IsActive = discount.IsActive;
                    existingDiscount.DiscountPercentage = discount.DiscountPercentage;

                    Console.WriteLine($"Updated existing discount for ProductId {discount.ProductId}, DiscountId: {existingDiscount.DiscountId}");
                }
                else
                {
                    var newDiscount = new DiscountModel
                    {
                        ProductId = discount.ProductId,
                        DiscountPercentage = discount.DiscountPercentage,
                        StartDate = discount.StartDate,
                        EndDate = discount.EndDate,
                        IsActive = discount.IsActive,
                        Description = $"Discount applied to product {product.Name}"
                    };

                    _dbContext.Discounts.Add(newDiscount);
                    Console.WriteLine($"Added new discount for ProductId {discount.ProductId}");
                }
            }
            else
            {
                Console.WriteLine($"Product with ProductId {discount.ProductId} not found.");
            }
        }

        try
        {
            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Database changes saved.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving changes to database: {ex.Message}");
        }
    }
}