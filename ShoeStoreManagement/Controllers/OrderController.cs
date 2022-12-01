using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Enums;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModel;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Security.Claims;
using ValueType = ShoeStoreManagement.Core.Enums.ValueType;

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
        private readonly IAddressCRUD _addressCRUD;
        private readonly ICartDetailCRUD _cartDetailCRUD;
        private readonly IApplicationUserCRUD _applicationUserCRUD;
        private static OrderVM _orderVM = new OrderVM();

        public OrderController(IOrderCRUD orderCRUD, IAddressCRUD addressCRUD, IOrderDetailCRUD orderDetailCRUD, IProductCRUD productCRUD, ICartCRUD cartCRUD, ICartDetailCRUD cartDetailCRUD, IVoucherCRUD voucherCRUD, ISizeDetailCRUD sizeDetailCRUD, IApplicationUserCRUD applicationUserCRUD)
        {
            _orderCRUD = orderCRUD;
            _orderDetailCRUD = orderDetailCRUD;
            _productCRUD = productCRUD;
            _cartCRUD = cartCRUD;
            _cartDetailCRUD = cartDetailCRUD;
            _voucherCRUD = voucherCRUD;
            _sizeDetailCRUD = sizeDetailCRUD;

            _orderVM.deliveryMethods = Enum.GetValues(typeof(DeliveryMethods)).Cast<DeliveryMethods>().ToList();
            _orderVM.paymentMethods = Enum.GetValues(typeof(PaymentMethod)).Cast<PaymentMethod>().ToList();
            _applicationUserCRUD = applicationUserCRUD;
            _addressCRUD= addressCRUD;
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
            _orderVM.currOrder.User = _applicationUserCRUD.GetByIdAsync(userId).Result;
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
                    Size = item.Size,
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
            _orderVM.currOrder.DeliverryMethods = orderVm.currOrder.DeliverryMethods;
            _orderVM.currOrder.PhoneNumber = orderVm.currOrder.PhoneNumber;
            _orderVM.currOrder.DeliveryAddress = orderVm.currOrder.DeliveryAddress;

            if (orderVm.currOrder.DeliverryMethods == DeliveryMethods.Fast)
            {
                _orderVM.currOrder.DeliveryCharge = 5;
            }
            _orderVM.currOrder.OrderTotalPrice = _orderVM.currOrder.OrderTotalPayment + _orderVM.currOrder.DeliveryCharge;

            return View(_orderVM);
        }

        [HttpPost]
        public string CheckVouncher(string id)
        {
            // Handle if having voucher code
            if (!string.IsNullOrEmpty(id))
            {
                List<Voucher> vouchers = _voucherCRUD.GetAllAsync().Result;
                Voucher currentVoucher = null;

                foreach (Voucher voucher in vouchers)
                {
                    if (id.Equals(voucher.Code))
                    {
                        currentVoucher = voucher;
                        break;
                    }
                }

                if (currentVoucher != null)
                {
                    // Handle voucher's type
                    switch (currentVoucher.ConditionType)
                    {
                        case ConditionType.MinPrice:
                            {
                                // Debug 01: TotalPayment = 0
                                if (_orderVM.currOrder.OrderTotalPayment < float.Parse(currentVoucher.ConditionValue))
                                {
                                    // Annouce the total price does not satisfy
                                    return "Condition not match!";
                                }
                                break;
                            }
                        case ConditionType.NewCustomer:
                            {
                                if (_orderCRUD.GetAllAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result != null)
                                {
                                    // Annouce that user is not New Customer (first time ordering)
                                    return "Condition not match!";
                                }
                                break;
                            }

                    }

                    // Handle if expired
                    switch (currentVoucher.ExpiredType)
                    {
                        case ExpireType.ExpiredDate:
                            {
                                DateTime expiredDate = DateTime.ParseExact(currentVoucher.ExpiredValue, "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                                if (DateTime.Now > expiredDate)
                                {
                                    // Annouce that voucer has been expired
                                    return "Expired!";
                                }
                                break;
                            }
                        case ExpireType.Amount:
                            {
                                if (int.Parse(currentVoucher.ExpiredValue) <= 0)
                                {
                                    // Annouce that voucer has been sold out
                                    return "Out of vouncher!";
                                }
                                else
                                {
                                    currentVoucher.ExpiredValue = (int.Parse(currentVoucher.ExpiredValue) - 1).ToString();
                                }
                                break;
                            }
                    }

                    // Handle voucher value type
                    switch (currentVoucher.ValueType)
                    {
                        case ValueType.RealValue:
                            {
                                _orderVM.currOrder.OrderTotalPrice -= currentVoucher.Value;
                                break;
                            }
                        case ValueType.Percent:
                            {
                                _orderVM.currOrder.OrderTotalPrice -= _orderVM.currOrder.OrderTotalPrice * currentVoucher.Value / 100;
                                break;
                            }
                    }
                    _orderVM.currOrder.OrderVoucherId = currentVoucher.Id;
                    return "valid";
                }
                else
                {
                    // Annouce there is no valid voucher
                    return "invalid";
                }
            }
            return "invalid";
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderVM? orderVM)
        {
            if (orderVM.currOrder == null)
            {
                return NotFound();
            }

            // Handle if having voucher code
            if (!string.IsNullOrEmpty(orderVM.currOrder.OrderVoucher.Code))
            {
                List<Voucher> vouchers = _voucherCRUD.GetAllAsync().Result;
                Voucher currentVoucher = null;

                foreach (Voucher voucher in vouchers)
                {
                    if (orderVM.currOrder.OrderVoucher.Code.Equals(voucher.Code))
                    {
                        currentVoucher = voucher;
                        break;
                    }
                }

                if (currentVoucher != null)
                {
                    // Handle voucher's type
                    switch (currentVoucher.ConditionType)
                    {
                        case ConditionType.MinPrice:
                            {
                                // Debug 01: TotalPayment = 0
                                if (_orderVM.currOrder.OrderTotalPayment < float.Parse(currentVoucher.ConditionValue))
                                {
                                    // Annouce the total price does not satisfy
                                    return RedirectToAction("Index", "Cart");
                                }
                                break;
                            }
                        case ConditionType.NewCustomer:
                            {
                                if (_orderCRUD.GetAllAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result != null)
                                {
                                    // Annouce that user is not New Customer (first time ordering)
                                    return RedirectToAction("Index", "Cart");
                                }
                                break;
                            }

                    }

                    // Handle if expired
                    switch (currentVoucher.ExpiredType)
                    {
                        case ExpireType.ExpiredDate:
                            {
                                DateTime expiredDate = DateTime.ParseExact(currentVoucher.ExpiredValue, "MM-dd-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                                if (DateTime.Now > expiredDate)
                                {
                                    // Annouce that voucer has been expired
                                    return RedirectToAction("Index", "Cart");
                                }
                                break;
                            }
                        case ExpireType.Amount:
                            {
                                if (int.Parse(currentVoucher.ExpiredValue) <= 0)
                                {
                                    // Annouce that voucer has been sold out
                                    return RedirectToAction("Index", "Cart");
                                }
                                else
                                {
                                    currentVoucher.ExpiredValue = (int.Parse(currentVoucher.ExpiredValue) - 1).ToString();
                                }
                                break;
                            }
                    }

                    // Handle voucher value type
                    switch (currentVoucher.ValueType)
                    {
                        case ValueType.RealValue:
                            {
                                _orderVM.currOrder.OrderTotalPrice -= currentVoucher.Value;
                                break;
                            }
                        case ValueType.Percent:
                            {
                                _orderVM.currOrder.OrderTotalPrice -= _orderVM.currOrder.OrderTotalPrice * currentVoucher.Value / 100;
                                break;
                            }
                    }
                    _orderVM.currOrder.OrderVoucherId = currentVoucher.Id;
                }
                else
                {
                    // Annouce there is no valid voucher
                    return RedirectToAction("Index", "Cart");
                }
            }


            _orderVM.currOrder.PaymentMethod = _orderVM.currOrder.PaymentMethod;
            _orderVM.currOrder.TotalAmount = _orderVM.totalAmount;

            _orderVM.currOrder.User = null;

            await _orderCRUD.CreateAsync(_orderVM.currOrder);

            Cart? cart = _cartCRUD.GetAsync(_orderVM.currOrder.UserId).Result;

            if (cart == null)
            {
                return NotFound();
            }


            foreach (var item in _orderVM.currOrder.OrderDetails)
            {
                item.Product = null;

                await _orderDetailCRUD.CreateAsync(item);

                CartDetail? cartDetail = await _cartDetailCRUD.GetByProductIdAsync(item.ProductId, cart.CartId, item.Size);

                var ob = _productCRUD.GetByIdAsync(item.ProductId);

                if (cartDetail != null)
                {
                    item.Product = ob.Result;
                    _cartDetailCRUD.Remove(cartDetail.CartDetailId);
                }
                SizeDetail? sizeDetail = await _sizeDetailCRUD.GetProductSizeAsync(item.ProductId, item.Size);

                if (sizeDetail != null)
                {
                    if (sizeDetail.Amount >= item.Amount)
                    {
                        sizeDetail.Amount -= item.Amount;
                        await _sizeDetailCRUD.Update(sizeDetail);
                    }
                    else
                    {
                        // Thong bao len la khong du so luong
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult OrderDetail(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order? order = _orderCRUD.GetByIdAsync(id).Result;

            if (order != null)
            {
                List<OrderDetail> orderDetails = _orderDetailCRUD.GetAllAsync(id).Result;

                if (orderDetails != null)
                {
                    foreach (var item in orderDetails)
                    {
                        item.Product = _productCRUD.GetByIdAsync(item.ProductId).Result;
                        order.OrderDetails.Add(item);
                    }
                }

                _orderVM.currOrder = order;

                return View(_orderVM);
            }

            return NotFound();
        }
    }
}
