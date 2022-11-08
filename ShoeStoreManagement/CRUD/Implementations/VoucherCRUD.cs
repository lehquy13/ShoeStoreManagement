using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;

namespace ShoeStoreManagement.CRUD.Implementations
{
	public class VoucherCRUD : IVoucherCRUD
	{
        private readonly ApplicationDbContext _applicationDBContext;

        public VoucherCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;

        }

        public async Task CreateAsync(Voucher voucher)
		{
            await _applicationDBContext.Vouchers.AddAsync(voucher);
            _applicationDBContext.SaveChanges();
        }

		public async Task<List<Voucher>> GetAllAsync()
		{
            return await _applicationDBContext.Vouchers.ToListAsync();
        }

		public async Task<Voucher?> GetByIdAsync(string id)
		{
            return await _applicationDBContext.Vouchers.FindAsync(id);
        }

		public void Remove(Voucher deteleVoucher)
		{
            if (deteleVoucher != null)
                _applicationDBContext.Vouchers.Remove(deteleVoucher);
            _applicationDBContext.SaveChanges();
        }

		public void Update(Voucher updateVoucher)
		{
            if (updateVoucher != null)
                _applicationDBContext.Vouchers.Update(updateVoucher);
            _applicationDBContext.SaveChanges();
        }
	}
}
