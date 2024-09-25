namespace WebStoreHubAPI.Dtos
{
    public class ProductCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public IFormFile ImageUrl { get; set; }

        public int ProductTypeId { get; set; }
    }
}
