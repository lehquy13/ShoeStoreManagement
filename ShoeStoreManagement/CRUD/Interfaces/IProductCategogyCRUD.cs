using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Areas.Admin.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Data;
using System.Xml.Linq;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface IProductCategoryCRUD
    {
        public Task<List<ProductCategory>> GetAllAsync();
        public Task<ProductCategory?> GetByIdAsync(string id);
        public Task CreateAsync(ProductCategory productCategory);
        public void Update(ProductCategory updateProductCategory);
        public void Remove(ProductCategory deteleProductCategory);
    }
}
