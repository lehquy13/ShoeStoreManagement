using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Core.Models
{
    public class CartDetail
    {
        string cartDetailId = string.Empty;
        string cartId = string.Empty;
        string productId = string.Empty;
        Product? product;
        int amount = 0;
        int totalSum = 0;

        [Key]
        public string CartDetailId
        {
            get { return cartDetailId; }
            set { }
        }

        [Required]
        [ForeignKey("Cart")]
        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        [Required]
        [ForeignKey("Product")]
        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public Product? Product
        {
            get { return product; }
            set { product = value; }
        }

        [Range(0, 99999)]
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        [Range(0, 99999)]
        [DataType(DataType.Currency)]
        public int CartDetailTotalSum
        {
            get { return totalSum; }
            set { totalSum = value; }
        }
    }
}
