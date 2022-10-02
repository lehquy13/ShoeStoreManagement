

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Models
{
    public class Product
    {
        string productId = Guid.NewGuid().ToString();
        string productName = String.Empty;
        string productCategoryId = String.Empty;
        int productUnitPrice = 0;
        float productDiscount = 0;

        [Key]
        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        [Required]
        [ForeignKey("ProductCategoryId")]
        public string ProductCategoryId
        {
            get { return productCategoryId; }
            set { productCategoryId = value; }
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
    }
}
