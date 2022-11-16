using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class WishListDetailCRUD : IWishListDetailCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;
        public WishListDetailCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        public async Task CreateAsync(WishListDetail wishListDetail)
        {
            await _applicationDBContext.WishListDetails.AddAsync(wishListDetail);
            _applicationDBContext.SaveChanges();
        }

        public async Task<List<WishListDetail>> GetAllAsync(string wishListId)
        {
            return await _applicationDBContext.WishListDetails.Where(x => x.WishListId == wishListId).ToListAsync();
        }

        public async Task<WishListDetail?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.WishListDetails.Where(x => x.WishListDetailId == id).FirstOrDefaultAsync();
        }

        public async Task<WishListDetail?> GetByProductIdAsync(string wishListId, string productId)
        {
            return await _applicationDBContext.WishListDetails.Where(x => x.WishListId == wishListId && x.ProductId == productId).FirstOrDefaultAsync();
        }

        public void Remove(WishListDetail deteleWishListDetail)
        {
            if (deteleWishListDetail != null)
                _applicationDBContext.WishListDetails.Remove(deteleWishListDetail);
            _applicationDBContext.SaveChanges();
        }

        public void Update(WishListDetail updateWishListDetail)
        {
            if (updateWishListDetail != null)
                _applicationDBContext.WishListDetails.Update(updateWishListDetail);
            _applicationDBContext.SaveChanges();
        }
    }
}
