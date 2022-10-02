
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Models
{
    public class OrderDetail
    {
        string orderDetailId = Guid.NewGuid().ToString();
        string orderId = String.Empty;
        string productId = String.Empty;
        int payment = 0;

        [Key]
        public string OrderDetailId
        {
            get { return orderDetailId; }
            set { orderDetailId = value; }
        }

        [Required]
        [ForeignKey("OrderId")]
        public string OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        [Required]
        [ForeignKey("ProductId")]
        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public int Payment
        {
            get { return payment; }
            set { payment = value; }
        }
        
    }
}
