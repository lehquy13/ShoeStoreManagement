using Microsoft.AspNetCore.Identity;
using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    // Add property for user
    
    public Cart? Cart { get; set; }

    public List<Address> addresses { get; set; } = new List<Address>();
}

