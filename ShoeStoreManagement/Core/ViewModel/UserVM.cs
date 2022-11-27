using Microsoft.AspNetCore.Identity;
using ShoeStoreManagement.Areas.Identity.Data;

namespace ShoeStoreManagement.Core.ViewModel
{
	public class UserVM
	{
		public ApplicationUser? user { get; set; }

		public List<ApplicationUser>? applicationUsers { get; set; }

		public List<IdentityRole>? roles { get; set; }

		public List<string>? applicationuserRoles { get; set; }

    }
}
