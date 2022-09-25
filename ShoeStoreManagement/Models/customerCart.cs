using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class CustomerCart
    {
        string cartId = String.Empty;
        string customerId = String.Empty;
        int cartTotalPrice = 0;

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        public string CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }
        public int CartTotalPrice
        {
            get { return cartTotalPrice; }
            set { cartTotalPrice = value; }
        }
    }
}
