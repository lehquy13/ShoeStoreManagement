using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;
using System.Data.Entity;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class AddressCRUD : IAddressCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;

        public AddressCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        public async Task CreateAsync(Address address)
        {
            await _applicationDBContext.Addresses.AddAsync(address);
            _applicationDBContext.SaveChanges();
        }

        public void DeleteAllAdressByIdAsync(string id)
        {
            var obj = _applicationDBContext.Addresses.Where(b => b.UserId == id).ToArray<Address>();
            _applicationDBContext.Addresses.RemoveRange(obj);

        }

        public async Task<List<Address>> GetAllAsync(string userId)
        {
            return await _applicationDBContext.Addresses.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Address?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.Addresses.FindAsync(id);
        }

        public void Remove(Address deteleAddress)
        {
            if (deteleAddress != null)
                _applicationDBContext.Addresses.Remove(deteleAddress);
            _applicationDBContext.SaveChanges();
        }

        public void Update(Address updateAddress)
        {
            if (updateAddress != null)
                _applicationDBContext.Addresses.Update(updateAddress);
            _applicationDBContext.SaveChanges();
        }
    }
}
