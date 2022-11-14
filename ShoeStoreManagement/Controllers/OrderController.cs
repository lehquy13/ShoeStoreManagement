using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Enums;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModels;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Security.Claims;

namespace ShoeStoreManagement.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderCRUD _orderCRUD;
        private readonly IOrderDetailCRUD _orderDetailCRUD;
        private readonly IProductCRUD _productCRUD;
        private readonly IVoucherCRUD _voucherCRUD;
        private readonly ICartCRUD _cartCRUD;
        private readonly ICartDetailCRUD _cartDetailCRUD;
        private static OrderVM _orderVM = new OrderVM();

        public OrderController(IOrderCRUD orderCRUD, IOrderDetailCRUD orderDetailCRUD, IProductCRUD productCRUD, ICartCRUD cartCRUD, ICartDetailCRUD cartDetailCRUD, IVoucherCRUD voucherCRUD)
        {
            _orderCRUD = orderCRUD;
            _orderDetailCRUD = orderDetailCRUD;
            _productCRUD = productCRUD;
            _cartCRUD = cartCRUD;
            _cartDetailCRUD = cartDetailCRUD;
            _voucherCRUD = voucherCRUD;
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

        // get id of cart
        [HttpGet]
        public IActionResult MakeAnOrder()
        {
            List<Voucher>? vouchers = _voucherCRUD.GetAllAsync().Result;

            ViewData["vouchers"] = vouchers;
            ViewData["paymentMethods"] = Enum.GetValues(typeof(PaymentMethod)).Cast<PaymentMethod>().ToList();
            ViewData["deliveryMethods"] = Enum.GetValues(typeof(DeliveryMethods)).Cast<DeliveryMethods>().ToList();


            // create fake order
            OrderDetail orderDetail = new OrderDetail();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart? cart = _cartCRUD.GetAsync(userId).Result;

            if(cart == null)
            {
                return NotFound();
            }

            var list = _cartDetailCRUD.GetAllCheckedAsync(cart.CartId).Result;

            if(list.Count <= 0)
            {
                return NotFound();
            }

            _orderVM.currOrder.OrderDetails.Clear();

            foreach(var item in list)
            {
                orderDetail = new OrderDetail() {
                    Amount = item.Amount,
                    OrderId = _orderVM.currOrder.OrderId,
                    Payment = (int)item.CartDetailTotalSum,
                    ProductId = item.ProductId,
                };

                _orderVM.currOrder.OrderDetails.Add(orderDetail);
            }

            return View(_orderVM.currOrder);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(_orderVM.currOrder);
        }

        [HttpPost]
        public IActionResult Create(Order? order)
        {
            if(order == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            order.UserId = userId;

            _orderCRUD.CreateAsync(order);
            
            if (_orderVM.currOrder.OrderDetails.Count > 0)
            {
                foreach (var item in _orderVM.currOrder.OrderDetails)
                {
                    _orderDetailCRUD.CreateAsync(item);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
