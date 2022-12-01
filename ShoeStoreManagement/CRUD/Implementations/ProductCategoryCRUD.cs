using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Areas.Admin.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class ProductCategoryCRUD : IProductCategoryCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;
        public ProductCategoryCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<List<ProductCategory>> GetAllAsync()
        {
            return await _applicationDBContext.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.ProductCategories.FindAsync(id);
        }

        public async Task CreateAsync(ProductCategory productCategory)
        {
            await _applicationDBContext.ProductCategories.AddAsync(productCategory);
            _applicationDBContext.SaveChanges();
        }

        public void Update(ProductCategory updateProductCategory)
        {
            var o = this.GetByIdAsync(updateProductCategory.ProductCategoryId).Result;
            if (updateProductCategory != null)
            {
                o.ProductCategoryName = updateProductCategory.ProductCategoryName;
            }
                //_applicationDBContext.ProductCategories.Update(updateProductCategory);
            _applicationDBContext.SaveChanges();
        }

        public void Remove(ProductCategory deteleProductCategory)
        {
            if (deteleProductCategory != null)
                _applicationDBContext.ProductCategories.Remove(deteleProductCategory);
            _applicationDBContext.SaveChanges();
        }
    }
}
