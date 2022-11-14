using Microsoft.AspNetCore.Identity;
using ShoeStoreManagement.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Areas.Identity.Data;


public class ApplicationUser : IdentityUser
{
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [NotMapped]
    public string SingleAddress { get; set; } = string.Empty;

    [NotMapped]
    public string Role { get; set; } = string.Empty;

    [NotMapped]
    public Cart? Cart { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime Birthday { get; set; }

    public List<Address> Addresses { get; set; } = new List<Address>();

    //[NotMapped]
    //public bool? isCheck { get; set; } = false;
}
