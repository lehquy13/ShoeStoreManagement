using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class ProductCategory
    {
        string productCategoryId = String.Empty;
        string productCategoryName = String.Empty;

       
        public string ProductCategoryId
        {
            get { return productCategoryId; }
            set { productCategoryId = value; }
        }
        public string ProductCategoryName
        {
            get { return productCategoryName; }
            set { productCategoryName = value; }
        }
    }
}
