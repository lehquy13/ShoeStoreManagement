using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class WishListCRUD : IWishListCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;

        public WishListCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        public async Task CreateAsync(WishList wishList)
        {
            await _applicationDBContext.WishLists.AddAsync(wishList);
            _applicationDBContext.SaveChanges();
        }

        public async Task<WishList?> GetAsync(string userId)
        {
            return await _applicationDBContext.WishLists.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<WishList?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.WishLists.Where(x => x.WishListId == id).FirstOrDefaultAsync();
        }

        public void Remove(WishList deteleWishList)
        {
            if (deteleWishList != null)
                _applicationDBContext.WishLists.Remove(deteleWishList);
            _applicationDBContext.SaveChanges();
        }

        public void Update(WishList updateWishList)
        {
            if (updateWishList != null)
                _applicationDBContext.WishLists.Update(updateWishList);
            _applicationDBContext.SaveChanges();
        }
    }
}
