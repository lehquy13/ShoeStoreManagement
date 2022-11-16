using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Core.Models
{
    public class WishListDetail
    {
        [Key]
        public string WishListDetailId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey("WishList")]
        public string WishListId { get; set; } = String.Empty;

        [Required]
        public string ProductId { get; set; } = String.Empty;

        public Product? Product { get; set; } = null;
    }
}
