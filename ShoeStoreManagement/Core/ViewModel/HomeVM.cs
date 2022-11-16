using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.Core.ViewModel
{
	public class HomeVM
	{
		public List<Product>? products { get; set; } = null;

		public WishList? wishList { get; set; } = null;

		public List<WishListDetail>? wishListDetails { get; set; } = null;
	}
}
