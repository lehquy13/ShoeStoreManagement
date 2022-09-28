using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class CustomerCart
    {
        Guid cartId = Guid.Empty;
        Guid customerId = Guid.Empty;
        int cartTotalPrice = 0;

        public Guid CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        public Guid CustomerId
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
