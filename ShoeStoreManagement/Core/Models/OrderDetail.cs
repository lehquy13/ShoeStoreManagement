﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Core.Models
{
    public class OrderDetail
    {
        string orderDetailId = Guid.NewGuid().ToString();
        string orderId = string.Empty;
        string productId = string.Empty;
        Product? product;
        int amount = 0;
        int payment = 0;

        [Key]
        public string OrderDetailId
        {
            get { return orderDetailId; }
            set { }
        }

        [Required]
        [ForeignKey("Order")]
        public string OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        [Required]
        [ForeignKey("Product")]
        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        public Product? Product
        {
            get { return product; }
            set { product = value; }
        }
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public int Payment
        {
            get { return payment; }
            set { payment = value; }
        }



    }
}