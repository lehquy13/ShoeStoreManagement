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
            //var obj =  _applicationDBContext.Products.FindAsync(updateProduct.ProductId).Result;
            //obj = updateProduct;
            var result = _applicationDBContext.Products.SingleOrDefault(b => b.ProductId == updateProduct.ProductId);

            if (result != null)
            {
                result.ProductName = updateProduct.ProductName;
                result.ProductUnitPrice = updateProduct.ProductUnitPrice;
                result.Amount = updateProduct.Amount;
                result.ProductDiscount = updateProduct.ProductDiscount;
                result.ProductCategory = updateProduct.ProductCategory;
                result.ProductCategoryId = updateProduct.ProductCategoryId;
                result.Color = updateProduct.Color;
                
                _applicationDBContext.SaveChanges();
            }

            //_applicationDBContext.Products.Update(updateProduct);
            
            //_applicationDBContext.Entry(obj).CurrentValues.SetValues(updateProduct);
        }

        public void Remove(Product deteleProduct)
        {
            if (deteleProduct != null)
                _applicationDBContext.Products.Remove(deteleProduct);
            _applicationDBContext.SaveChanges();
        }
    }
}
