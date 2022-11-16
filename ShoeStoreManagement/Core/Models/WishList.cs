using ShoeStoreManagement.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Core.Models
{
    public class WishList
    {
        [Key]
        public string WishListId { get; set; } = Guid.NewGuid().ToString();

        [NotMapped]
        public List<WishListDetail>? WishListDetails { get; set; } = null;

        [Required]
        public string UserId { get; set; } = String.Empty;

        public ApplicationUser? User { get; set; } = null;
    }
}
