using MongoDB.Bson.Serialization.Attributes;


namespace ShoeStoreManagement.Models
{
    public class ReceiptDetail
    {
        string receiptDetailId = String.Empty;
        string receiptId = String.Empty;
        string customerId = String.Empty;
        string staffId = String.Empty;
        string cartId = String.Empty;
        DateTime receiptDetailDate = DateTime.Now;

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ReceiptDetailId
        {
            get { return receiptDetailId; }
            set { receiptDetailId = value; }
        }
        public string ReceiptId
        {
            get { return receiptId; }
            set { receiptId = value; }
        }
        public string CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }
        public string StaffId
        {
            get { return staffId; }
            set { staffId = value; }
        }
        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        public DateTime ReceiptDetailDate
        {
            get { return receiptDetailDate; }
            set { receiptDetailDate = value; }
        }
    }
}
