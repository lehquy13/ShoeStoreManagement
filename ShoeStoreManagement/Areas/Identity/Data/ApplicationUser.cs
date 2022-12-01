using Microsoft.AspNetCore.Identity;
using ShoeStoreManagement.Core;
using ShoeStoreManagement.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Areas.Identity.Data;


public class ApplicationUser : IdentityUser
{
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Required]
    public string SingleAddress { get; set; } = "";

	[Required]
    [RegularExpression(@"^[a-zA-Z]*$",ErrorMessage = "Invalid fullname")]
	public string FullName { get; set; } = "";

    [Required]
    public string Role { get; set; } = "Customer";

    [NotMapped]
    public Cart? Cart { get; set; }

    [MinimunYear(1960)]
    [DataType(DataType.DateTime)]
    [Required]
    public DateTime Birthday { get; set; }

    public List<Address> Addresses { get; set; } = new List<Address>();

    public string AvatarName { get; set; } = "";

    [NotMapped]
    public IFormFile Avatar { get; set; }

    [RegularExpression(@"^([\+]?84[-]?|[0])?[1-9][0-9]{8}$",ErrorMessage = "Invalid Phone Numbber!")]
    public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
}
