using System.ComponentModel.DataAnnotations;

namespace WebStoreHubAPI.Models
{
    public class OrderModel
    {
        [Key]
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }
    }
}
