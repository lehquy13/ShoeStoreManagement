using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class Product
    {
        string productId = String.Empty;
        string productName = String.Empty;
        string categoryId = String.Empty;
        int productUnitPrice = 0;
        float productDiscount = 0;

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
        public string CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
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
