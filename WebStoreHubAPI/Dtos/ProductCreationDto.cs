namespace WebStoreHubAPI.Dtos
{
    public class ProductCreationDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public int ProductTypeId { get; set; }

        public int BrandId { get; set; }
    }
}
