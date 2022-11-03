using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Areas.Admin.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Data;
using System.Xml.Linq;

namespace ShoeStoreManagement.CRUD
{
    public class ProductCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;
        public ProductCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
            
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return  await _applicationDBContext.Products.ToListAsync<Product>();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.Products.FindAsync(id);
        }

        public async Task CreateAsync(Product productCategory)
        {
            await _applicationDBContext.Products.AddAsync(productCategory);
            this._applicationDBContext.SaveChanges();
        }

        public void Update(Product updateProduct)
        {
            
            if (updateProduct != null)
                _applicationDBContext.Products.Update(updateProduct);
            this._applicationDBContext.SaveChanges();
        }

        public void RemoveAProduct(Product deteleProduct)
        {
            if (deteleProduct != null)
                _applicationDBContext.Products.Remove(deteleProduct);
            this._applicationDBContext.SaveChanges();
        }
    }
}
