using Microsoft.AspNetCore.Mvc;


using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModel;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;
using ShoeStoreManagement.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ShoeStoreManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductCRUD _productCRUD;
        private readonly IWishListCRUD _wishListCRUD;
        private readonly IWishListDetailCRUD _wishListDetailCRUD;
        private readonly IImageCRUD _imageCRUD;
        private readonly IProductCategoryCRUD _productCategoryCRUD;
        private static ProductVM _productVM = new ProductVM();

        public HomeController(IProductCRUD productCRUD, IWishListCRUD wishListCRUD, IWishListDetailCRUD wishListDetailCRUD, IImageCRUD imageCRUD, IProductCategoryCRUD productCategoryCRUD)
        {
            _productCRUD = productCRUD;
            _wishListCRUD = wishListCRUD;
            _wishListDetailCRUD = wishListDetailCRUD;
            _imageCRUD = imageCRUD;
            _productCategoryCRUD = productCategoryCRUD;
        }

        [HttpGet]
        public IActionResult Index(string categoryRadio, string priceRadio, int page = 1)
        {
            ViewBag.Home = true;

            _productVM.filters = new List<string>();
            _productVM.filters.Add(categoryRadio);
            _productVM.filters.Add(priceRadio);
            _productVM.productCategories = _productCategoryCRUD.GetAllAsync().Result;

            List<Product> list = _productCRUD.GetAllAsync().Result;

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            WishList? wishList = _wishListCRUD.GetAsync(userId).Result;

            if (wishList != null)
            {
                foreach (var item in list)
                {
                    if (_wishListDetailCRUD.GetByProductIdAsync(wishList.WishListId, item.ProductId).Result != null)
                    {
                        item.IsLiked = true;
                    }

                    List<Image> img = _imageCRUD.GetAllByProductIdAsync(item.ProductId).Result;

                    if ( img.Count > 0 )
                    {
                        item.ImageName = img[0].ImageName;
                    }
                }
            }

            list = list.OrderBy(i => i.ProductName).ToList();
            
            _productVM.products = list;

            _productVM.page = 0;
            ViewData["nProducts"] = _productVM.page;
            return View(_productVM);
        }

        [HttpPost]
        public IActionResult Sort(ProductVM productVM)
        {
            List<Product> productFilter = new List<Product>();
            _productVM.filters = new List<string>();
            _productVM.filters.Add(productVM.categoryRadio);
            _productVM.filters.Add(productVM.priceRadio);
            _productVM.products = _productCRUD.GetAllAsync().Result;

            for (int i = 0; i < _productVM.products.Count; i++)
            {
                _productVM.products[i].SetCategory(_productVM.productCategories);
            }

            float minvalue = -1, maxvalue = -1;

            if (_productVM.products.Count > 0)
            {
                if (!string.IsNullOrEmpty(productVM.priceRadio) && !productVM.priceRadio.Equals("All"))
                {
                    string[] strsplt = productVM.priceRadio.Split('-', 2, StringSplitOptions.None);
                    minvalue = float.Parse(strsplt[0]);
                    maxvalue = float.Parse(strsplt[1]);
                }
            }

            foreach (var i in _productVM.products)
            {
                if ((i.ProductCategory.ProductCategoryName.Equals(productVM.categoryRadio)
                    || string.IsNullOrEmpty(productVM.categoryRadio)) && minCheck(minvalue, i.ProductUnitPrice)
                    && maxCheck(maxvalue, i.ProductUnitPrice))
                {
                    if (string.IsNullOrEmpty(_productVM.searchString))
                        productFilter.Add(i);
                    else if (!string.IsNullOrEmpty(_productVM.searchString.ToLower()) && i.ProductName.ToLower().Contains(_productVM.searchString.ToLower()))
                        productFilter.Add(i);
                }
            }

            productFilter = productFilter.OrderBy(i => i.ProductName).ToList();
            _productVM.products = productFilter;

            _productVM.page = productVM.page - 1;

            ViewData["nProducts"] = _productVM.page;
            return PartialView("_ViewAll", _productVM);
        }

        [HttpPost]
        public IActionResult Pagination(int page = 1)
        {
            _productVM.page = page - 1;
            ViewData["nProducts"] = _productVM.page;
            return PartialView("_ViewAll", _productVM);
        }

        [HttpPost]
        public IActionResult Search(ProductVM productVM)
        {
            List<Product> productFilter = new List<Product>();
            _productVM.products = _productCRUD.GetAllAsync().Result;

            for (int i = 0; i < _productVM.products.Count; i++)
            {
                _productVM.products[i].SetCategory(_productVM.productCategories);
            }

            float minvalue = -1, maxvalue = -1;

            if (!string.IsNullOrEmpty(_productVM.filters[1]) && !_productVM.filters[1].Equals("All"))
            {
                string[] strsplt = _productVM.filters[1].Split('-', 2, StringSplitOptions.None);
                minvalue = float.Parse(strsplt[0]);
                maxvalue = float.Parse(strsplt[1]);
            }

            if (!string.IsNullOrEmpty(productVM.searchString))
            {
                foreach (var i in _productVM.products)
                {
                    if ((i.ProductCategory.ProductCategoryName.Equals(_productVM.filters[0])
                    || string.IsNullOrEmpty(_productVM.filters[0])) && minCheck(minvalue, i.ProductUnitPrice)
                    && maxCheck(maxvalue, i.ProductUnitPrice) && i.ProductName.ToLower().Contains(productVM.searchString.ToLower()))
                        productFilter.Add(i);
                }
                productFilter = productFilter.OrderBy(i => i.ProductName).ToList();
                _productVM.products = productFilter;
                _productVM.page = 0;
            }
            else
            {
                foreach (var i in _productVM.products)
                {
                    if ((i.ProductCategory.ProductCategoryName.Equals(_productVM.filters[0])
                    || string.IsNullOrEmpty(_productVM.filters[0])) && minCheck(minvalue, i.ProductUnitPrice)
                    && maxCheck(maxvalue, i.ProductUnitPrice))
                        productFilter.Add(i);
                }

                productFilter = productFilter.OrderBy(i => i.ProductName).ToList();
                _productVM.products = productFilter;
                _productVM.page = 0;
            }
            _productVM.searchString = productVM.searchString;
            ViewData["nProducts"] = _productVM.page;
            return PartialView("_ViewAll", _productVM);
        }

        private bool minCheck(float value, float price)
        {
            if (value == -1)
                return true;

            if (price >= value)
                return true;
            else
                return false;
        }

        private bool maxCheck(float value, float price)
        {
            if (value == -1)
                return true;

            if (price < value)
                return true;
            else
                return false;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult WishList()
        {
            ViewBag.WishList = true;

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            WishList? wishList = _wishListCRUD.GetAsync(userId).Result;

            if (wishList != null)
            {
                wishList.WishListDetails = _wishListDetailCRUD.GetAllAsync(wishList.WishListId).Result;

                foreach (var item in wishList.WishListDetails)
                {
                    item.Product = _productCRUD.GetByIdAsync(item.ProductId).Result;
                }
            }

            return View(wishList);
        }

        [HttpGet("Home/AddToWishList/{id}")]
        public IActionResult AddToWishList(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Product? product = _productCRUD.GetByIdAsync(id).Result;

            if (product == null)
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            WishList? wishList = _wishListCRUD.GetAsync(userId).Result;

            WishListDetail wishListDetail = new WishListDetail();

            if (wishList == null)
            {
                wishList = new WishList()
                {
                    UserId = userId,
                };

                _wishListCRUD.CreateAsync(wishList);
            }

            wishListDetail.WishListId = wishList.WishListId;
            wishListDetail.ProductId = product.ProductId;

            _wishListDetailCRUD.CreateAsync(wishListDetail);

            return RedirectToAction("Index");
        }

        [HttpGet("Home/RemoveFromWishList/{id}")]
        public IActionResult RemoveFromWishList(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            WishList? wishList = _wishListCRUD.GetAsync(userId).Result;

            if (wishList == null)
            {
                return NotFound();
            }

            WishListDetail? wishListDetail = _wishListDetailCRUD.GetByProductIdAsync(wishList.WishListId, id).Result;

            if (wishListDetail == null)
            {
                return NotFound();
            }

            _wishListDetailCRUD.Remove(wishListDetail);

            return RedirectToAction("WishList");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}