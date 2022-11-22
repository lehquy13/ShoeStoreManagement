using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Areas.Admin.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class SizeDetailCRUD : ISizeDetailCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;
        public SizeDetailCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<List<SizeDetail>> GetAllAsync()
        {
            return await _applicationDBContext.SizeDetails.ToListAsync();
        }
        public async Task<List<SizeDetail>> GetAllByIdAsync(string id)
        {

            return await _applicationDBContext.SizeDetails
                .Where(b => b.ProductId == id)
                .ToListAsync<SizeDetail>();
        }

        public void DeleteAllDetailsByIdAsync(string id)
        {

            var obj = _applicationDBContext.SizeDetails.Where(b => b.ProductId == id).ToArray<SizeDetail>();
            _applicationDBContext.SizeDetails.RemoveRange(obj);
            _applicationDBContext.SaveChanges();
                
        }

        public async Task<SizeDetail?> GetProductSizeAsync(string id, int size)
        {
            return await _applicationDBContext.SizeDetails.Where(x=> x.ProductId == id && x.Size == size).FirstOrDefaultAsync();
        }

        public async Task<SizeDetail?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.SizeDetails.FindAsync(id);
        }

        public async Task CreateAsync(SizeDetail sizeDetail)
        {
            await _applicationDBContext.SizeDetails.AddAsync(sizeDetail);
            _applicationDBContext.SaveChanges();
        }

        public void Update(SizeDetail updateSizeDetail)
        {
            if(updateSizeDetail != null)
            {
                var obj = this.GetProductSizeAsync(updateSizeDetail.ProductId, updateSizeDetail.Size).Result;
                if (obj != null)
                {
                    obj.Amount = updateSizeDetail.Amount;
                    _applicationDBContext.SaveChanges();

                }
            }
           
        }

        public void Remove(SizeDetail deteleSizeDetail)
        {
            var obj = this.GetProductSizeAsync(deteleSizeDetail.ProductId, deteleSizeDetail.Size).Result;
            if (obj != null)
                _applicationDBContext.SizeDetails.Remove(obj);
            _applicationDBContext.SaveChanges();
        }

       
    }
}
