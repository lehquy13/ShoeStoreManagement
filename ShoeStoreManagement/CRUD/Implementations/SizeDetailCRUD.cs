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

            if (updateSizeDetail != null)
                _applicationDBContext.SizeDetails.Update(updateSizeDetail);
            _applicationDBContext.SaveChanges();
        }

        public void Remove(SizeDetail deteleSizeDetail)
        {
            if (deteleSizeDetail != null)
                _applicationDBContext.SizeDetails.Remove(deteleSizeDetail);
            _applicationDBContext.SaveChanges();
        }

        
    }
}
