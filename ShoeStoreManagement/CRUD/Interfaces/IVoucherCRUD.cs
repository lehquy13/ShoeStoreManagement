using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.CRUD.Interfaces
{
	public interface IVoucherCRUD
	{
        public Task<List<Voucher>> GetAllAsync();
        public Task<Voucher?> GetByIdAsync(string id);
        public Task CreateAsync(Voucher voucher);
        public void Update(Voucher updateVoucher);
        public void Remove(Voucher deteleVoucher);
    }
}
