
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Models
{
    public class Order
    {
        string orderId = Guid.NewGuid().ToString();
       List<OrderDetail> orderDetails = new List<OrderDetail>();
        string customerId = String.Empty;
        DateTime orderDate = DateTime.Now;
        int orderTotalPayment = 0;
        string orderVoucherId = String.Empty;
        string orderNote = String.Empty;

        [Key]
        public string OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        [Required]
        [ForeignKey("CustomerId")]
        public string CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }

        public List<OrderDetail> OrderDetails
        {
            get { return orderDetails; }
            set { orderDetails = value; }
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
