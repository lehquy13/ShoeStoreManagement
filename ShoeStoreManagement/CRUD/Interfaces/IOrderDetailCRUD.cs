using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface IOrderDetailCRUD
    {
        public Task<List<OrderDetail>> GetAllAsync(string orderId);
        public Task<OrderDetail?> GetByIdAsync(string id);
        public Task CreateAsync(OrderDetail orderDetail);
        public void Update(OrderDetail updateOrderDetail);
        public void Remove(OrderDetail deteleOrderDetail);
    }
}
