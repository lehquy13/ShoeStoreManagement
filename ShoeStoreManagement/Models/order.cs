using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    internal class Order
    {
        string orderId = String.Empty;
        string cartId = String.Empty;
        DateTime orderDate = DateTime.Now;
        int orderTotalPayment = 0;
        string orderVoucherId = String.Empty;
        string orderNote = String.Empty;

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }
        public string CartId
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
        public string OrderVoucherId
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
