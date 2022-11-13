using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Security.Claims;

namespace ShoeStoreManagement.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartCRUD _cartCRUD;

        private readonly ICartDetailCRUD _cartDetailCRUD;

        private readonly IProductCRUD _productCRUD;

        public CartController(ICartCRUD cartCRUD, ICartDetailCRUD cartDetailCRUD, IProductCRUD productCRUD)
        {
            _cartCRUD = cartCRUD;
            _cartDetailCRUD = cartDetailCRUD;
            _productCRUD = productCRUD;
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

        [HttpGet("Cart/Create/{id}")]
        public IActionResult Create(string? id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart? cart = _cartCRUD.GetAsync(userId).Result;

            if (id == null)
            {
                return NotFound();
            }

            if (cart == null)
            {
                cart = new Cart();
                cart.UserId = userId;
                _cartCRUD.CreateAsync(cart);
            }

            Product? product = _productCRUD.GetByIdAsync(id).Result;

            if (product == null)
            {
                return NotFound();
            }

            var cartDetail = new CartDetail()
            {
                CartId = cart.CartId,
                ProductId = id,
                Amount = 1,
                CartDetailTotalSum = product.ProductUnitPrice,
            };

            _cartDetailCRUD.CreateAsync(cartDetail);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("Cart/Edit/{id}/{amount}/{sum}")]
        public IActionResult Edit(string id, int amount, int sum)
        {
            CartDetail? cartDetail = _cartDetailCRUD.GetByIdAsync(id).Result;

            if (cartDetail == null) { return NotFound(); }

            cartDetail.Amount = amount;
            cartDetail.CartDetailTotalSum = sum;

            _cartDetailCRUD.Update(cartDetail);

            return RedirectToAction("Index");
        }

        [HttpGet("Cart/Delete/{id}")]
        public IActionResult Delete(string id)
        {
            CartDetail? cartDetail = _cartDetailCRUD.GetByIdAsync(id).Result;

            if (cartDetail == null) { return NotFound(); }

            _cartDetailCRUD.Remove(cartDetail);

            return RedirectToAction("Index");
        }
    }
}
