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
                // Reading data from the Excel file and mapping it to the DiscountDto
                var discountDto = new DiscountDto
                {
                    ProductId = int.Parse(worksheet.Cells[row, 1].Text), // Assuming ProductId is in the first column
                    DiscountPercentage = decimal.TryParse(worksheet.Cells[row, 2].Text, out var percentage) ? percentage : 0, // Assuming DiscountPercentage is in the second column
                    DiscountAmount = decimal.TryParse(worksheet.Cells[row, 3].Text, out var amount) ? amount : 0, // Assuming DiscountAmount is in the third column
                    StartDate = DateTime.Parse(worksheet.Cells[row, 4].Text), // Assuming StartDate is in the fourth column
                    EndDate = DateTime.Parse(worksheet.Cells[row, 5].Text), // Assuming EndDate is in the fifth column
                    IsActive = bool.Parse(worksheet.Cells[row, 6].Text) // Assuming IsActive is in the sixth column
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
            if (product != null && discount.IsActive)
            {
                decimal newDiscountedPrice = product.Price; // Start with the original price

                // Apply discount logic to calculate the new discounted price
                if (discount.DiscountPercentage > 0)
                {
                    var discountAmount = product.Price * (discount.DiscountPercentage / 100);
                    newDiscountedPrice -= discountAmount;
                }
                else if (discount.DiscountAmount > 0)
                {
                    newDiscountedPrice -= discount.DiscountAmount;
                }

                // Ensure the discounted price doesn't drop below zero
                if (newDiscountedPrice < 0) newDiscountedPrice = 0;

                // Update the discounted price instead of the original price
                product.DiscountedPrice = newDiscountedPrice;

                // Save the product price update
                _dbContext.DbProducts.Update(product);

                // Create a new DiscountModel and add it to the Discounts table
                var discountModel = new DiscountModel
                {
                    ProductId = discount.ProductId,
                    DiscountPercentage = discount.DiscountPercentage,
                    DiscountAmount = discount.DiscountAmount,
                    StartDate = discount.StartDate,
                    EndDate = discount.EndDate,
                    IsActive = discount.IsActive,
                    Description = $"Discount applied to product {product.Name}" // You can set a custom description
                };

                _dbContext.Discounts.Add(discountModel); // Save the discount details in the Discount table
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}