using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class Receipt
    {
        string receiptId = String.Empty;
        string cartId = String.Empty;
        DateTime receiptDate = DateTime.Now;
        int receiptTotalPayment = 0;
        string receiptVoucherId = String.Empty;
        string receiptNote = String.Empty;

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ReceiptId
        {
            get { return receiptId; }
            set { receiptId = value; }
        }
        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        public DateTime ReceiptDate
        {
            get { return receiptDate; }
            set { receiptDate = value; }
        }
        public int ReceiptTotalPayment
        {
            get { return receiptTotalPayment; }
            set { receiptTotalPayment = value; }
        }
        public string ReceiptVoucherId
        {
            get { return receiptVoucherId; }
            set { receiptVoucherId = value; }
        }
        public string ReceiptNote
        {
            get { return receiptNote; }
            set { receiptNote = value; }
        }
    }
}
