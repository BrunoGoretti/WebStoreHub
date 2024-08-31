using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class ProductService : IProductService
    {
        private List<Product> _products = new List<Product>();

        public void AddProduct(int productId, string name, string description, decimal price, int stock, string imageUrl)
        {

        }
    }
}
