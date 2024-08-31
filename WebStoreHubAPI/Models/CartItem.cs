namespace WebStoreHubAPI.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        // Navigation property to establish relationship with the Product entity
        public Product Product { get; set; }
    }
}
