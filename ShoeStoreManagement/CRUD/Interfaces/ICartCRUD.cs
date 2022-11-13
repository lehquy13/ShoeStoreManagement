using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface ICartCRUD
    {
        public Task<Cart?> GetAsync(string userId);
        public Task<Cart?> GetByIdAsync(string id);
        public Task CreateAsync(Cart cart);
        public void Update(Cart updateCart);
        public void Remove(Cart deteleCart);
    }
}
