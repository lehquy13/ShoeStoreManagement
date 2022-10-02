using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Models
{
    public class CartDetail
    {
        string cartDetailId = String.Empty;
        string cartId = String.Empty;
        string productId = String.Empty;
        int cartDetailAmount = 0;
        int cartDetailTotalSum = 0;
        Product product = new Product();

        [Key]
        public string CartDetailId
        {
            get { return cartDetailId; }
            set { cartDetailId = value; }
        }

        [Required]
        [ForeignKey("CartId")]
        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        [Required]
        [ForeignKey("ProductId")]
        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        public int CartDetailAmount
        {
            get { return cartDetailAmount; }
            set { cartDetailAmount = value; }
        }
        public int CartDetailTotalSum
        {
            get { return cartDetailTotalSum; }
            set { cartDetailTotalSum = value; }
        }
    }
}
