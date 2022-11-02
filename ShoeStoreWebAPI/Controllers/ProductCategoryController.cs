using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Data;

namespace ShoeStoreWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        ApplicationDbContext _applicationDbContext;


        public ProductCategoryController(ILogger<ProductController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet(Name = "GetAllCategory")]
        public List<ProductCategory> GetAllAsync()
        {
            return _applicationDbContext.ProductCategories.ToList<ProductCategory>();
        }

        [HttpGet("id")]
        public ProductCategory? GetOneAsync(string id)
        {
            return _applicationDbContext.ProductCategories.Find(id);
        }

        [HttpPost(Name = "PostOneCategory")]
        public void CreateACategory(ProductCategory productCategory)
        {
            _applicationDbContext.ProductCategories.Add(productCategory);
            this._applicationDbContext.SaveChanges();
        }

        //[HttpPut(Name = "PutOneCategory")] Becareful with this method, it wont work well for our logic
        //public void UpdateCategory(string id,ProductCategory productCategory)
        //{
        //    _applicationDbContext.ProductCategories.Update(productCategory);
        //}

        [HttpDelete(Name = "PostOneCategory")]
        public void RemoveACategory(string id)
        {
            var obj = _applicationDbContext.ProductCategories.Find(id);
            if(obj != null)
                _applicationDbContext.ProductCategories.Remove(obj);
            this._applicationDbContext.SaveChanges();
        }
    }
}