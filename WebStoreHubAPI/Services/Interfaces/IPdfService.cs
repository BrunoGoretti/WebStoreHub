using WebStoreHubAPI.Models;

public interface IPdfService
{
    byte[] GenerateOrderPdf(OrderModel order);
}