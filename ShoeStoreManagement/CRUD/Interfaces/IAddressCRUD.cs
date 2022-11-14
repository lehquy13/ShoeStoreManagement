using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface IAddressCRUD
    {
        public Task<List<Address>> GetAllAsync(string userId);
        public Task<Address?> GetByIdAsync(string id);

        public Task CreateAsync(Address address);
        public void Update(Address updateAddress);
        public void Remove(Address deteleAddress);
        public void DeleteAllAdressByIdAsync(string id);
    }
}
