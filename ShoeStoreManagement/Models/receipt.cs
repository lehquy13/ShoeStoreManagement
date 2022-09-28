using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class Receipt
    {
        Guid receiptId = Guid.Empty;
        Guid cartId = Guid.Empty;
        DateTime receiptDate = DateTime.Now;
        int receiptTotalPayment = 0;
        Guid receiptVoucherId = Guid.Empty;
        string receiptNote = String.Empty;

        public Guid ReceiptId
        {
            get { return receiptId; }
            set { receiptId = value; }
        }
        public Guid CartId
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
        public Guid ReceiptVoucherId
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
