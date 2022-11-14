using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface IOrderCRUD
    {
        public Task<List<Order>> GetAllOrderAsync();
        public Task<List<Order>> GetAllAsync(string userId);
        public Task<Order?> GetByIdAsync(string id);
        public Task CreateAsync(Order order);
        public void Update(Order updateOrder);
        public void Remove(Order deteleOrder);
    }
}
