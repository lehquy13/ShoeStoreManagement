using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Core.Models
{
    public class CartDetail
    {
        string cartDetailId = Guid.NewGuid().ToString();
        string cartId = string.Empty;
        string productId = string.Empty;
        Product? product;
        int amount = 0;
        float totalSum = 0;

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

        [Range(1, 99999)]
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        [Range(35, 45)]
        public int Size { get; set; } = 0;

        [Range(0, 99999)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public float CartDetailTotalSum
        {
            get { return totalSum; }
            set { totalSum = value; }
        }

        [Required]
        public bool IsChecked { get; set; } = false;
    }
}
