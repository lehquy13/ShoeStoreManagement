using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Areas.Admin.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;

namespace ShoeStoreManagement.CRUD.Implementations
{
    public class ProductCRUD : IProductCRUD
    {
        private readonly ApplicationDbContext _applicationDBContext;
        public ProductCRUD(ApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _applicationDBContext.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _applicationDBContext.Products.FindAsync(id);
        }

        public async Task CreateAsync(Product product)
        {
            await _applicationDBContext.Products.AddAsync(product);
            _applicationDBContext.SaveChanges();
        }

        public void Update(Product updateProduct)
        {
            if (updateProduct != null)
            {
                var obj = this.GetByIdAsync(updateProduct.ProductId).Result;
                if (obj != null)
                {
                    obj.ProductUnitPrice = updateProduct.ProductUnitPrice;
                    obj.ProductCategoryId = updateProduct.ProductCategoryId;
                    obj.Color = updateProduct.Color;
                    obj.Description= updateProduct.Description;
                    obj.ProductName = updateProduct.ProductName;
                   
                }

            }
                //_applicationDBContext.Products.Update(updateProduct);
            _applicationDBContext.SaveChanges();
        }

        public void Remove(Product deteleProduct)
        {
            var obj = _applicationDBContext.Products.FindAsync(deteleProduct.ProductId).Result;
            if (obj != null)
                _applicationDBContext.Products.Remove(obj);
            _applicationDBContext.SaveChanges();
        }
    }
}
