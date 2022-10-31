using ShoeStoreManagement.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShoeStoreManagement.Core.Models;

public class Cart
{
    string cartId = Guid.NewGuid().ToString();
    List<CartDetail> cartDetails = new List<CartDetail>();
    string userId = string.Empty;
    ApplicationUser? user = null;
    int cartTotalPrice = 0;


    [Key]
    public string CartId
    {
        get { return cartId; }
        set { }
    }

    [Required]
    [ForeignKey("Id")]
    public string UserId
    {
        get { return userId; }
        set { userId = value; }
    }

    public ApplicationUser? User
    {
        get { return user; }
        set { user = value; }
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
