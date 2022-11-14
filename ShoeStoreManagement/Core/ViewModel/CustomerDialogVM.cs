
using ShoeStoreManagement.Core.Enums;
using ShoeStoreManagement.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.Core.ViewModels;

public class CustomerDialogVM
{
    
    public string pickCustomerId { get; set; } = "";
    
    public List<ApplicationUser>? customers { get; set; } = new List<ApplicationUser>();
    
    public ApplicationUser pickCustomers { get; set; } = new ApplicationUser();


}
