using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Areas.Admin.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Data;
using System.Xml.Linq;

namespace ShoeStoreManagement.CRUD.Interfaces
{
    public interface IProductCRUD
    {
        public Task<List<Product>> GetAllAsync();
        public Task<Product?> GetByIdAsync(string id);
        public Task CreateAsync(Product productCategory);
        public void Update(Product updateProduct);
        public void RemoveAProduct(Product deteleProduct);
    }
}
