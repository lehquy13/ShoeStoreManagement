using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Areas.Admin.Controllers;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Data;
using System.Xml.Linq;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface IApplicationUserCRUD
    {
        public Task<List<ApplicationUser>> GetAllAsync();
        public Task<ApplicationUser?> GetByIdAsync(string id);
        public Task CreateAsync(ApplicationUser applicationUser);
        public void Update(ApplicationUser updateApplicationUser);
        public void Remove(ApplicationUser deleteApplicationUser);
    }
}
