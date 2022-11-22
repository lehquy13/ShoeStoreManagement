using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class CartDetailCRUD : ICartDetailCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;
        public CartDetailCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        public async Task CreateAsync(CartDetail cartDetail)
        {
            await _applicationDBContext.CartDetails.AddAsync(cartDetail);
            _applicationDBContext.SaveChanges();
        }

        public async Task<List<CartDetail>> GetAllAsync(string cartId)
        {
            return await _applicationDBContext.CartDetails.Where(x => x.CartId == cartId).ToListAsync();
        }

        public async Task<List<CartDetail>> GetAllCheckedAsync(string cartId)
        {
            return await _applicationDBContext.CartDetails.Where(x => x.CartId == cartId && x.IsChecked == true).ToListAsync();
        }

        public async Task<CartDetail?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.CartDetails.FindAsync(id);
        }

        public async Task<CartDetail?> GetByProductIdAsync(string id, string cartId, int size)
        {
            return await _applicationDBContext.CartDetails.Where(x => x.CartId == cartId && x.ProductId == id && x.Size == size).FirstOrDefaultAsync();
        }

        public void Remove(CartDetail deteleCartDetail)
        {
            var obj = _applicationDBContext.CartDetails.FindAsync(deteleCartDetail.CartDetailId).Result;
            if (obj != null)
            {
                //deteleCartDetail.Product = deteleCartDetail.Product;
                _applicationDBContext.CartDetails.Remove(obj);
            }

            _applicationDBContext.SaveChanges();
        }

        public void Remove(string id)
        {
            var obj = _applicationDBContext.CartDetails.FindAsync(id).Result;
            if (obj != null)
            {
                _applicationDBContext.CartDetails.Remove(obj);
                _applicationDBContext.SaveChanges();

            }

        }
        public void Update(CartDetail updateCartDetail)
        {
            if (updateCartDetail != null)
                _applicationDBContext.CartDetails.Update(updateCartDetail);
            _applicationDBContext.SaveChanges();
        }
    }
}
