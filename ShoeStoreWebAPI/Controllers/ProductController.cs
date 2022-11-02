using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Data;

namespace ShoeStoreWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        ApplicationDbContext _applicationDbContext;


        public ProductController(ILogger<ProductController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet(Name = "GetAllProduct")]
        public List<Product> GetAllAsync()
        {
            return _applicationDbContext.Products.ToList<Product>();
        }

        [HttpGet("id")]
        public Product? GetOneAsync(string id)
        {
            return _applicationDbContext.Products.Find(id);
        }

        [HttpPost(Name = "PostOneProduct")]
        public void CreateACategory(Product productCategory)
        {
            _applicationDbContext.Products.Add(productCategory);
            this._applicationDbContext.SaveChanges();
        }

        //[HttpPut(Name = "PutOneCategory")] Becareful with this method, it wont work well for our logic
        //public void UpdateCategory(string id,ProductCategory productCategory)
        //{
        //    _applicationDbContext.ProductCategories.Update(productCategory);
        //}

        [HttpDelete(Name = "PostOneProduct")]
        public void RemoveAProduct(string id)
        {
            var obj = _applicationDbContext.Products.Find(id);
            if (obj != null)
                _applicationDbContext.Products.Remove(obj);
            this._applicationDbContext.SaveChanges();
        }
    }
}