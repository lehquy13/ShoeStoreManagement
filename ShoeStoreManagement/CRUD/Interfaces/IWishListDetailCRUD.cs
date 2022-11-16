using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface IWishListDetailCRUD
    {
        public Task<List<WishListDetail>> GetAllAsync(string wishListId);
        public Task<WishListDetail?> GetByIdAsync(string id);
        public Task<WishListDetail?> GetByProductIdAsync(string wishListId, string productId);
        public Task CreateAsync(WishListDetail wishListDetail);
        public void Update(WishListDetail updateWishListDetail);
        public void Remove(WishListDetail deteleWishListDetail);
    }
}
