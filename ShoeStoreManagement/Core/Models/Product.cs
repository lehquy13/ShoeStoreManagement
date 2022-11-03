﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Mapping;

namespace ShoeStoreManagement.Core.Models
{
    public class Product
    {
        string productId = Guid.NewGuid().ToString();
        string productName = string.Empty;
        string productCategoryId = string.Empty;
        string productCategory = string.Empty;
        int productUnitPrice = 0;
        float productDiscount = 0;
        string color = string.Empty;
        List<string> sizes = new List<string>();

        [Key]
        public string ProductId
        {
            get { return productId; }
            private set { }
        }
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        [Required]
        [ForeignKey("ProductCategory")]
        public string ProductCategoryId
        {
            get { return productCategoryId; }
            private set { productCategoryId = value; }
        }
        public string ProductCategory
        {
            get { return productCategory; }
            set { productCategory = value; }
        }
        public int ProductUnitPrice
        {
            get { return productUnitPrice; }
            set { productUnitPrice = value; }
        }
        public float ProductDiscount
        {
            get { return productDiscount; }
            set { productDiscount = value; }
        }
       
        public string Color
        {
            get { return color; }
            set { color = value; }
        }
        [NotMapped]
        public List<string> Sizes
        {
            get { return sizes; }
            set { sizes = value; }
        }

        public bool SetCategory(List<ProductCategory> strings)
        {
            foreach (var obj in strings)
            {
                if(this.ProductCategoryId == obj.ProductCategoryId)
                {
                    this.ProductCategory = obj.ProductCategoryName;
                    return true;
                }
            }
            return false;
        }

    }
}
