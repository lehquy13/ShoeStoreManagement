using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;
using System.Data.Entity;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class SupplierCRUD : ISupplierCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;

        public SupplierCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        public async Task CreateAsync(Supplier supplier)
        {
            await _applicationDBContext.Suppliers.AddAsync(supplier);
            _applicationDBContext.SaveChanges();
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            return await _applicationDBContext.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.Suppliers.FindAsync(id);
        }

        public void Remove(Supplier deteleSupplier)
        {
            if (deteleSupplier != null)
                _applicationDBContext.Suppliers.Remove(deteleSupplier);
            _applicationDBContext.SaveChanges();
        }

        public void Update(Supplier updateSupplier)
        {
            if (updateSupplier != null)
                _applicationDBContext.Suppliers.Update(updateSupplier);
            _applicationDBContext.SaveChanges();
        }
    }
}
