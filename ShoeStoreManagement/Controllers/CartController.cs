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
                cart.CartTotalPrice = 0;
                cart.CartTotalAmountSelected = 0;
                cart.CartTotalAmount = 0;

                foreach (var detail in cart.CartDetails)
                {
                    detail.Product = _productCRUD.GetByIdAsync(detail.ProductId).Result;

                    if (detail.IsChecked)
                    {
                        cart.CartTotalAmountSelected += detail.Amount;
                        cart.CartTotalPrice += detail.CartDetailTotalSum;
                    }

                    cart.CartTotalAmount += detail.Amount;
                }

                return View(cart);
            }

            return View(new Cart());
        }

        [HttpPost]
        public void UpdateAmount(string id, int amount, int sum)
        {
            CartDetail? cartDetail = _cartDetailCRUD.GetByIdAsync(id).Result;

            if (cartDetail == null) { return; }

            var oldAmount = cartDetail.Amount;
            var oldPrice = cartDetail.CartDetailTotalSum;

            cartDetail.Amount = amount;
            cartDetail.CartDetailTotalSum = sum;

            _cartDetailCRUD.Update(cartDetail);

            Cart? cart = _cartCRUD.GetByIdAsync(cartDetail.CartId).Result;

            if (cart == null) { return; }

            if (cartDetail.IsChecked)
            {
                cart.CartTotalPrice -= oldPrice;
                cart.CartTotalPrice += sum;
                cart.CartTotalAmountSelected -= oldAmount;
                cart.CartTotalAmountSelected += amount;
            }

            cart.CartTotalAmount -= oldAmount;
            cart.CartTotalAmount += amount;

            _cartCRUD.Update(cart);

            return;
        }

        [HttpPost]
        public void Delete(string id)
        {
            CartDetail? cartDetail = _cartDetailCRUD.GetByIdAsync(id).Result;

            if (cartDetail == null) { return; }

            _cartDetailCRUD.Remove(cartDetail);

            Cart? cart = _cartCRUD.GetByIdAsync(cartDetail.CartId).Result;

            if (cart == null) { return; }

            if (cartDetail.IsChecked)
            {
                cart.CartTotalPrice -= cartDetail.CartDetailTotalSum;
                cart.CartTotalAmountSelected -= cartDetail.Amount;
            }

            return;
        }

        [HttpPost]
        public void UpdateChecked(string id, bool isChecked)
        {
            CartDetail? cartDetail = _cartDetailCRUD.GetByIdAsync(id).Result;

            if (cartDetail == null) { return; }

            cartDetail.IsChecked = isChecked;

            _cartDetailCRUD.Update(cartDetail);

            Cart? cart = _cartCRUD.GetByIdAsync(cartDetail.CartId).Result;

            if (cart == null) { return; }

            if (isChecked == false)
            {
                cart.CartTotalPrice -= cartDetail.CartDetailTotalSum;
                cart.CartTotalAmountSelected -= cartDetail.Amount;
            }
            else
            {
                cart.CartTotalPrice += cartDetail.CartDetailTotalSum;
                cart.CartTotalAmountSelected += cartDetail.Amount;
            }

            return;
        }
    }
}
