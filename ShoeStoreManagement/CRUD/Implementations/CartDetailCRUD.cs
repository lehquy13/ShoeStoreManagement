using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;
using System.Data.Entity;

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

        public async Task<CartDetail?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.CartDetails.FindAsync(id);
        }

        public void Remove(CartDetail deteleCartDetail)
        {
            if (deteleCartDetail != null)
                _applicationDBContext.CartDetails.Remove(deteleCartDetail);
            _applicationDBContext.SaveChanges();
        }

        public void Update(CartDetail updateCartDetail)
        {
            if (updateCartDetail != null)
                _applicationDBContext.CartDetails.Update(updateCartDetail);
            _applicationDBContext.SaveChanges();
        }
    }
}
