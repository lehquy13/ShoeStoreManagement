using System.ComponentModel.DataAnnotations;

namespace ShoeStoreManagement.Core.Models
{
    public class ProductCategory
    {
        string productCategoryId = Guid.NewGuid().ToString();
        string productCategoryName = string.Empty;

        [Key]
        public string ProductCategoryId
        {
            get { return productCategoryId; }
            set { }
        }

        [Required]
        public string ProductCategoryName
        {
            get { return productCategoryName; }
            set { productCategoryName = value; }
        }
    }
}
