namespace WebStoreHubAPI.Dtos
{
    public class ProductCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public string ProductType { get; set; }
    }
}
