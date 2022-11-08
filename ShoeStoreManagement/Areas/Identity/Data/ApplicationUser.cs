using Microsoft.AspNetCore.Identity;
using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    // Add property for user
    DateTime createdDate = DateTime.Now;
    string singleAddress = string.Empty;
    public Cart? Cart { get; set; }

    public List<Address> addresses { get; set; } = new List<Address>();
    public DateTime CreatedDate { get => createdDate; set => createdDate = value; }
    public string SingleAddress { get => singleAddress; set => singleAddress = value; }
}

