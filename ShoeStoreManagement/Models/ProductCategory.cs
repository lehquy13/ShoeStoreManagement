using System.ComponentModel.DataAnnotations;

namespace ShoeStoreManagement.Models
{
    public class ProductCategory
    {
        string productCategoryId = Guid.NewGuid().ToString();
        string productCategoryName = String.Empty;

        [Key]
        public string ProductCategoryId
        {
            get { return productCategoryId; }
            set {  }
        }
        public string ProductCategoryName
        {
            get { return productCategoryName; }
            set { productCategoryName = value; }
        }
    }
}
