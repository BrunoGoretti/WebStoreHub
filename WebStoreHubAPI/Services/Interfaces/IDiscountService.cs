using WebStoreHubAPI.Dtos;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountDto>> ImportDiscountsFromExcelAsync(Stream excelStream);
        Task ApplyDiscountsAsync(IEnumerable<DiscountDto> discounts);
    }
}
