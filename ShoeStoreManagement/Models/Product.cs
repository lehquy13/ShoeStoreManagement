using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class Product
    {
        Guid productId = Guid.Empty;
        string productName = String.Empty;
        Guid categoryId = Guid.Empty;
        int productUnitPrice = 0;
        float productDiscount = 0;

        public Guid ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        public Guid CategoryId
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
