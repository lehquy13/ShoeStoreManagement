using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
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
        string description = "";

        int productUnitPrice = 0;
        float productDiscount = 0;
        string color = string.Empty;
        List<SizeDetail> sizes = new List<SizeDetail>() { };
        int amount = 0;

        [Key]
        public string ProductId
        {
            get { return productId; }
            set  { productId = value; }
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
            set { productCategoryId = value; }
        }
        [NotMapped]
        public string ProductCategory
        {
            get { return productCategory; }
            set { productCategory = value; }
        }

        [Range(0, 99999)]
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
        public List<SizeDetail> Sizes
        {
            get { return sizes; }
            set { sizes = value; }
        }

        [NotMapped]
        public List<string> TestSize { get; set; } = new List<string>();

        [NotMapped]
        public List<string> TestSizeAmount { get; set; } = new List<string>();

        [NotMapped]
        public Dictionary<int,int> SizeHashtable { get; set; } = new Dictionary<int, int>();

        [NotMapped]
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public bool SetCategory(List<ProductCategory> strings)
        {
            foreach (var obj in strings)
            {
                if (this.ProductCategoryId == obj.ProductCategoryId)
                {
                    this.ProductCategory = obj.ProductCategoryName;
                    return true;
                }
            }
            return false;
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

    }
}
