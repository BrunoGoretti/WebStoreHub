﻿using System.ComponentModel.DataAnnotations;

namespace WebStoreHubAPI.Models
{
    public class OrderItemModel
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public OrderModel Order { get; set; }
        public ProductModel Product { get; set; }
    }
}
