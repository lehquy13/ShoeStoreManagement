using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface IWishListCRUD
    {
        public Task<WishList?> GetAsync(string userId);
        public Task<WishList?> GetByIdAsync(string id);
        public Task CreateAsync(WishList wishList);
        public void Update(WishList updateWishList);
        public void Remove(WishList deteleWishList);
    }
}
