using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Security.Claims;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart? cart = (_cartCRUD.GetAsync(userId).Result != null) ? _cartCRUD.GetAsync(userId).Result : new Cart();

            if (cart != null)
            {
                cart.CartDetails = _cartDetailCRUD.GetAllAsync(cart.CartId).Result;

                foreach (var detail in cart.CartDetails)
                {
                    detail.Product = _productCRUD.GetByIdAsync(detail.ProductId).Result;
                }

                return View(cart);
            }

            return NotFound();
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
