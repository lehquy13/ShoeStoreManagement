using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface ISupplierCRUD
    {
        public Task<List<Supplier>> GetAllAsync();
        public Task<Supplier?> GetByIdAsync(string id);
        public Task CreateAsync(Supplier supplier);
        public void Update(Supplier updateSupplier);
        public void Remove(Supplier deteleSupplier); 
    }
}
