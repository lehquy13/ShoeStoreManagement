using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Areas.Admin.Controllers;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class ApplicationUserCRUD : IApplicationUserCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;
        public ApplicationUserCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await _applicationDBContext.ApplicationUsers.ToListAsync();
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.ApplicationUsers.FindAsync(id);
        }

        public async Task CreateAsync(ApplicationUser applicationUser)
        {
            await _applicationDBContext.ApplicationUsers.AddAsync(applicationUser);
            _applicationDBContext.SaveChanges();
        }

        public void Update(ApplicationUser updateApplicationUser)
        {
            if (updateApplicationUser == null) { return; }
            var obj = _applicationDBContext.ApplicationUsers.FindAsync(updateApplicationUser.Id).Result;
            if (obj != null)
            {
                obj.UserName = updateApplicationUser.UserName;
                obj.PhoneNumber = updateApplicationUser.PhoneNumber;
                obj.Email = updateApplicationUser.Email;
                obj.Birthday = updateApplicationUser.Birthday;
                obj.AvatarName = updateApplicationUser.AvatarName;
                obj.SingleAddress = updateApplicationUser.SingleAddress;
                
                // more and more
                _applicationDBContext.SaveChanges();

            }
        }

        public void Remove(ApplicationUser deleteApplicationUser)
        {
            if (deleteApplicationUser != null)
                _applicationDBContext.ApplicationUsers.Remove(deleteApplicationUser);
            _applicationDBContext.SaveChanges();
        }
    }
}
