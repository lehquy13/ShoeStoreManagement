using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;
using System.Data.Entity;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class OrderCRUD : IOrderCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;

        public OrderCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        public async Task CreateAsync(Order order)
        {
            await _applicationDBContext.Orders.AddAsync(order);
            _applicationDBContext.SaveChanges();
        }

        public async Task<List<Order>> GetAllAsync(string userId)
        {
            return await _applicationDBContext.Orders.Where(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.Orders.FindAsync(id);
        }

        public void Remove(Order deteleOrder)
        {
            if (deteleOrder != null)
                _applicationDBContext.Orders.Remove(deteleOrder);
            _applicationDBContext.SaveChanges();
        }

        public void Update(Order updateOrder)
        {
            if (updateOrder != null)
                _applicationDBContext.Orders.Update(updateOrder);
            _applicationDBContext.SaveChanges();
        }
    }
}
