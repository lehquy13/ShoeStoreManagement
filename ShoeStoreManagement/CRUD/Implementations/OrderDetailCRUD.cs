using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class OrderDetailCRUD : IOrderDetailCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;
        public OrderDetailCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        public async Task CreateAsync(OrderDetail orderDetail)
        {
            await _applicationDBContext.OrderDetails.AddAsync(orderDetail);
            _applicationDBContext.SaveChanges();
        }

        public async Task<List<OrderDetail>> GetAllAsync(string orderId)
        {
            return await _applicationDBContext.OrderDetails.Where(x => x.OrderId == orderId).ToListAsync(); 
        }

        public async Task<OrderDetail?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.OrderDetails.FindAsync(id);
        }

        public void Remove(OrderDetail deteleOrderDetail)
        {
            if (deteleOrderDetail != null)
                _applicationDBContext.OrderDetails.Remove(deteleOrderDetail);
            _applicationDBContext.SaveChanges();
        }

        public void Update(OrderDetail updateOrderDetail)
        {
            if (updateOrderDetail != null)
                _applicationDBContext.OrderDetails.Update(updateOrderDetail);
            _applicationDBContext.SaveChanges();
        }
    }
}
