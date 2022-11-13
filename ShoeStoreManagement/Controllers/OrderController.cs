using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Security.Claims;

namespace ShoeStoreManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderCRUD _orderCRUD;
        private readonly IOrderDetailCRUD _orderDetailCRUD;
        private readonly IProductCRUD _productCRUD;

        public OrderController(IOrderCRUD orderCRUD, IOrderDetailCRUD orderDetailCRUD, IProductCRUD productCRUD)
        {
            _orderCRUD = orderCRUD;
            _orderDetailCRUD = orderDetailCRUD;
            _productCRUD = productCRUD;
        }

        public IActionResult Index()
        {
            ViewBag.Order = true;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Order> orders = _orderCRUD.GetAllAsync(userId).Result;

            foreach (var order in orders)
            {
                order.OrderDetails = _orderDetailCRUD.GetAllAsync(order.OrderId).Result;

                foreach(var detail in order.OrderDetails)
                {
                    detail.Product = _productCRUD.GetByIdAsync(detail.ProductId).Result;
                }
            }

            return View(orders);
        }

        public IActionResult MakeAnOrder()
        {
            return View();
        }
    }
}
