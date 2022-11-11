using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class CartCRUD : ICartCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;

        public CartCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        public async Task CreateAsync(Cart cart)
        {
            await _applicationDBContext.Carts.AddAsync(cart);
            _applicationDBContext.SaveChanges();
        }

        public async Task<Cart> GetAsync(string userId)
        {
            return await _applicationDBContext.Carts.Where(o => o.UserId == userId).FirstAsync();
        }

        public async Task<Cart?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.Carts.FindAsync(id);
        }

        public void Remove(Cart deteleCart)
        {
            if (deteleCart != null)
                _applicationDBContext.Carts.Remove(deteleCart);
            _applicationDBContext.SaveChanges();
        }

        public void Update(Cart updateCart)
        {
            if (updateCart != null)
                _applicationDBContext.Carts.Update(updateCart);
            _applicationDBContext.SaveChanges();
        }
    }
}
