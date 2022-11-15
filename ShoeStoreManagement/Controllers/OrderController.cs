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

            _orderVM.currOrder.UserId = userId;
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
                _orderVM.currOrder.OrderTotalPayment += (int)item.CartDetailTotalSum;
            }

            return View(_orderVM);
        }

        [HttpPost]
        public IActionResult ConfirmOrder(OrderVM orderVm)
        {
            ViewData["paymentMethods"] = Enum.GetValues(typeof(PaymentMethod)).Cast<PaymentMethod>().ToList();

            _orderVM.currOrder.DeliverryMethods = orderVm.currOrder.DeliverryMethods;

            if (orderVm.currOrder.DeliverryMethods == DeliveryMethods.Fast)
            {
                _orderVM.currOrder.DeliveryCharge = 5;
            }
            _orderVM.currOrder.OrderTotalPrice = _orderVM.currOrder.OrderTotalPayment + _orderVM.currOrder.DeliveryCharge;

            return View(_orderVM);
        }

        [HttpPost]
        public IActionResult Create(OrderVM? orderVM)
        {
            if(orderVM.currOrder == null)
            {
                return NotFound();
            }

            _orderVM.currOrder.PaymentMethod = orderVM.currOrder.PaymentMethod;

            _orderCRUD.CreateAsync(_orderVM.currOrder);

            Cart? cart = _cartCRUD.GetAsync(_orderVM.currOrder.UserId).Result;

            if (cart == null)
            {
                return NotFound();
            }

            foreach(var item in _orderVM.currOrder.OrderDetails)
            {
                _orderDetailCRUD.CreateAsync(item);

                Product? product = _productCRUD.GetByIdAsync(item.ProductId).Result;
                CartDetail? cartDetail = _cartDetailCRUD.GetByProductIdAsync(item.ProductId, cart.CartId).Result;

                product.Amount -= item.Amount;

                if ( cartDetail != null )
                {
                    _productCRUD.Update(product);
                    _cartDetailCRUD.Remove(cartDetail);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
