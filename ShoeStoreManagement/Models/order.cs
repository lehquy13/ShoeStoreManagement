using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class Order
    {
        Guid orderId = Guid.Empty;
        Guid cartId = Guid.Empty;
        DateTime orderDate = DateTime.Now;
        int orderTotalPayment = 0;
        Guid orderVoucherId = Guid.Empty;
        string orderNote = String.Empty;

        
        public Guid OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }
        public Guid CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        public DateTime OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; }
        }
        public int OrderTotalPayment
        {
            get { return orderTotalPayment; }
            set { orderTotalPayment = value; }
        }
        public Guid OrderVoucherId
        {
            get { return orderVoucherId; }
            set { orderVoucherId = value; }
        }
        public string OrderNote
        {
            get { return orderNote; }
            set { orderNote = value; }
        }
    }
}
