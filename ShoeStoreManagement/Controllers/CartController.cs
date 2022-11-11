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
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart cart = _cartCRUD.GetAsync(userId).Result;

            cart.CartDetails = _cartDetailCRUD.GetAllAsync(cart.CartId).Result;            

            return View(cart);
        }
    }
}
