using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModels;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Security.Claims;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CartController : Controller
    {
        private const string role = "Customer";

        private readonly ICartCRUD _cartCRUD;

        private readonly ICartDetailCRUD _cartDetailCRUD;

        private readonly IProductCRUD _productCRUD;
        private readonly IApplicationUserCRUD _applicationUserCRUD;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ICartCRUD cartCRUD, ICartDetailCRUD cartDetailCRUD, IProductCRUD productCRUD, IApplicationUserCRUD applicationUserCRUD, UserManager<ApplicationUser> userManager)
        {
            _cartCRUD = cartCRUD;
            _cartDetailCRUD = cartDetailCRUD;
            _productCRUD = productCRUD;
            _applicationUserCRUD = applicationUserCRUD;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart? cart = _cartCRUD.GetAsync(userId).Result;

            if (cart != null)
            {
                cart.CartDetails = _cartDetailCRUD.GetAllAsync(cart.CartId).Result;

                foreach (var detail in cart.CartDetails)
                {
                    detail.Product = _productCRUD.GetByIdAsync(detail.ProductId).Result;
                }

                return View(cart);
            }

            return View(new Cart());
        }

        public IActionResult CartToOrder()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["pickedUser"] = _applicationUserCRUD.GetByIdAsync(userId).Result;
            
            return RedirectToAction("PickCustomer", "Order");
        }
        //[HttpGet("Cart/Create/{id}")]
        //public IActionResult Create(string? id)
        //{
        //    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    Cart? cart = _cartCRUD.GetAsync(userId).Result;

        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    if (cart == null)
        //    {
        //        cart = new Cart();
        //        cart.UserId = userId;
        //        _cartCRUD.CreateAsync(cart);
        //    }

        //    Product? product = _productCRUD.GetByIdAsync(id).Result;

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    CartDetail? cartDetail = _cartDetailCRUD.GetByProductIdAsync(id, cart.CartId).Result;

        //    if (cartDetail != null)
        //    {
        //        if (cartDetail.Amount < product.Amount)
        //        {
        //            cartDetail.Amount++;
        //            cartDetail.CartDetailTotalSum += cartDetail.Amount * product.ProductUnitPrice;
        //            _cartDetailCRUD.Update(cartDetail);
        //        }
        //    }
        //    else
        //    {
        //        cartDetail = new CartDetail()
        //        {
        //            CartId = cart.CartId,
        //            ProductId = id,
        //            Amount = 1,
        //            CartDetailTotalSum = product.ProductUnitPrice,
        //        };

        //        _cartDetailCRUD.CreateAsync(cartDetail);
        //    }

        //    return RedirectToAction("Index", "Home");
        //}

        [HttpGet("Admin/Cart/Edit/{id}/{amount}/{sum}")]
        public IActionResult Edit(string id, int amount, int sum)
        {
            CartDetail? cartDetail = _cartDetailCRUD.GetByIdAsync(id).Result;

            if (cartDetail == null) { return NotFound(); }

            cartDetail.Amount = amount;
            cartDetail.CartDetailTotalSum = sum;

            _cartDetailCRUD.Update(cartDetail);

            return RedirectToAction("Index");
        }

        [HttpGet("Admin/Cart/Delete/{id}")]
        public IActionResult Delete(string id)
        {
            CartDetail? cartDetail = _cartDetailCRUD.GetByIdAsync(id).Result;

            if (cartDetail == null) { return NotFound(); }

            _cartDetailCRUD.Remove(cartDetail);

            return RedirectToAction("Index");
        }

        [HttpGet("Admin/Cart/UpdateChecked/{id}/{isChecked}")]
        public IActionResult UpdateChecked(string id, bool isChecked)
        {
            CartDetail? cartDetail = _cartDetailCRUD.GetByIdAsync(id).Result;

            if (cartDetail == null) { return NotFound(); }

            cartDetail.IsChecked = isChecked;

            _cartDetailCRUD.Update(cartDetail);

            return RedirectToAction("Index");
        }
    }
}
