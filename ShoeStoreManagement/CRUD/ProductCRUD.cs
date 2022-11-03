using Microsoft.AspNetCore.Mvc;
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

        public List<Product> GetAllAsync()
        {
            return _applicationDbContext.Products.ToList<Product>();
        }

        public Product? GetOneAsync(string id)
        {
            return _applicationDbContext.Products.Find(id);
        }

        public void CreateACategory(Product productCategory)
        {
            _applicationDbContext.Products.Add(productCategory);
            this._applicationDbContext.SaveChanges();
        }

        public void UpdateCategory(string id)
        {
            var obj = _applicationDbContext.Products.Find(id);
            //obj.name = "";
            if (obj != null)
                _applicationDbContext.Products.Update(obj);
        }

        public void RemoveAProduct(string id)
        {
            var obj = _applicationDbContext.Products.Find(id);
            if (obj != null)
                _applicationDbContext.Products.Remove(obj);
            this._applicationDbContext.SaveChanges();
        }
    }
}
