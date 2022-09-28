using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class ProductCategory
    {
        Guid productCategoryId = Guid.Empty;
        string productCategoryName = String.Empty;

       
        public Guid ProductCategoryId
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
