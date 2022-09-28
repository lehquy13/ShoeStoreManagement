using MongoDB.Bson.Serialization.Attributes;


namespace ShoeStoreManagement.Models
{
    public class ReceiptDetail
    {
        Guid receiptDetailId = Guid.Empty;
        Guid receiptId = Guid.Empty;
        Guid customerId = Guid.Empty;
        Guid staffId = Guid.Empty;
        Guid cartId = Guid.Empty;
        DateTime receiptDetailDate = DateTime.Now;

        
        public Guid ReceiptDetailId
        {
            get { return receiptDetailId; }
            set { receiptDetailId = value; }
        }
        public Guid ReceiptId
        {
            get { return receiptId; }
            set { receiptId = value; }
        }
        public Guid CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }
        public Guid StaffId
        {
            get { return staffId; }
            set { staffId = value; }
        }
        public Guid CartId
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
