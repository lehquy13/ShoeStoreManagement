using Microsoft.AspNetCore.Mvc;


using Microsoft.EntityFrameworkCore;
using ShoeStoreManagement.Core.Models;
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

        public HomeController(IProductCRUD productCRUD, IWishListCRUD wishListCRUD, IWishListDetailCRUD wishListDetailCRUD, IImageCRUD imageCRUD)
        {
            _productCRUD = productCRUD;
            _wishListCRUD = wishListCRUD;
            _wishListDetailCRUD = wishListDetailCRUD;
            _imageCRUD = imageCRUD;
        }

        public IActionResult Index()
        {
            ViewBag.Home = true;

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

            return View(list);
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