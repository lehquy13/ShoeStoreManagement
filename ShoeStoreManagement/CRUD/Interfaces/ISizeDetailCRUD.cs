using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Areas.Admin.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Data;
using System.Xml.Linq;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface ISizeDetailCRUD
    {
        public Task<List<SizeDetail>> GetAllAsync();
        public Task<List<SizeDetail>> GetAllByIdAsync(string id);
        public Task<SizeDetail?> GetByIdAsync(string id);
        public Task CreateAsync(SizeDetail sizeDetail);
        public void Update(SizeDetail updateSizeDetail);
        public void Remove(SizeDetail deteleSizeDetail);
	}
}
