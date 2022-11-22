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
        private readonly ISizeDetailCRUD _sizeDetailCRUD;
        private readonly IVoucherCRUD _voucherCRUD;
        private readonly ICartCRUD _cartCRUD;
        private readonly ICartDetailCRUD _cartDetailCRUD;
        private static OrderVM _orderVM = new OrderVM();

        public OrderController(IOrderCRUD orderCRUD, IOrderDetailCRUD orderDetailCRUD, IProductCRUD productCRUD, ICartCRUD cartCRUD, ICartDetailCRUD cartDetailCRUD, IVoucherCRUD voucherCRUD, ISizeDetailCRUD sizeDetailCRUD)
        {
            _orderCRUD = orderCRUD;
            _orderDetailCRUD = orderDetailCRUD;
            _productCRUD = productCRUD;
            _cartCRUD = cartCRUD;
            _cartDetailCRUD = cartDetailCRUD;
            _voucherCRUD = voucherCRUD;
            _sizeDetailCRUD = sizeDetailCRUD;
        }

        public IActionResult Index()
        {
            ViewBag.Order = true;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Order> orders = _orderCRUD.GetAllAsync(userId).Result;

            foreach (var order in orders)
            {
                order.OrderDetails = _orderDetailCRUD.GetAllAsync(order.OrderId).Result;

                foreach (var detail in order.OrderDetails)
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

            _orderVM.currOrder = new Order();
            OrderDetail orderDetail = new OrderDetail();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart? cart = _cartCRUD.GetAsync(userId).Result;

            if (cart == null)
            {
                return NotFound();
            }

            var list = _cartDetailCRUD.GetAllCheckedAsync(cart.CartId).Result;

            if (list.Count <= 0)
            {
                return NotFound();
            }

            _orderVM.currOrder.UserId = userId;
            _orderVM.currOrder.OrderVoucherId = "";
            _orderVM.currOrder.OrderDetails.Clear();
            _orderVM.totalAmount = 0;

            foreach (var item in list)
            {
                Product? product = _productCRUD.GetByIdAsync(item.ProductId).Result;

                orderDetail = new OrderDetail()
                {
                    Amount = item.Amount,
                    OrderId = _orderVM.currOrder.OrderId,
                    Payment = (int)item.CartDetailTotalSum,
                    ProductId = item.ProductId,
                    Product = product,
                };

                _orderVM.totalAmount += item.Amount;
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
            if (orderVM.currOrder == null)
            {
                return NotFound();
            }

            _orderVM.currOrder.PaymentMethod = orderVM.currOrder.PaymentMethod;
            _orderVM.currOrder.TotalAmount = orderVM.totalAmount;

            _orderCRUD.CreateAsync(_orderVM.currOrder);

            Cart? cart = _cartCRUD.GetAsync(_orderVM.currOrder.UserId).Result;

            if (cart == null)
            {
                return NotFound();
            }

            foreach (var item in _orderVM.currOrder.OrderDetails)
            {
                _orderDetailCRUD.CreateAsync(item);

                CartDetail? cartDetail = _cartDetailCRUD.GetByProductIdAsync(item.ProductId, cart.CartId, item.Size).Result;

                // Subtract amount of size 
                SizeDetail? sizeDetail = _sizeDetailCRUD.GetProductSizeAsync(item.ProductId, item.Size).Result;

                if (sizeDetail != null)
                {
                    if (sizeDetail.Amount >= item.Amount)
                    {
                        sizeDetail.Amount -= item.Amount;
                        _sizeDetailCRUD.Update(sizeDetail);
                    }
                    else
                    {
                        // Thong bao len la khong du so luong
                    }
                }

                if (cartDetail != null)
                {
                    cartDetail.Product = null;
                    _cartDetailCRUD.Remove(cartDetail);
                }

            }

            return RedirectToAction("Index");
        }
    }
}
