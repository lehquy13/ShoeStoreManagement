
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Models
{
    public class Cart
    {
        string cartId = Guid.NewGuid().ToString();
        List<CartDetail> cartDetails = new List<CartDetail>();
        string customerId = string.Empty;
        int cartTotalPrice = 0;


        [Key]
        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        [Required]
        [ForeignKey("CustomerId")]
        public string CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }

        public List<CartDetail> CartDetails
        {
            get { return cartDetails; }
            set { cartDetails = value; }
        }

        public int CartTotalPrice
        {
            get { return cartTotalPrice; }
            set { cartTotalPrice = value; }
        }
    }
}
