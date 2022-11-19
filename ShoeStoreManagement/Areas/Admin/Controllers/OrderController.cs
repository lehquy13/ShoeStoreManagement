using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Enums;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModels;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Data;
using System.Security.Claims;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private const string role = "Customer";
        // DI
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderCRUD _orderCRUD;
        private readonly IOrderDetailCRUD _orderDetailCRUD;
        private readonly IApplicationUserCRUD _applicationuserCRUD;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IAddressCRUD _addressCRUD;
        private readonly IProductCRUD _productCRUD;
        private readonly ISizeDetailCRUD _sizeDetailCRUD;
        private readonly ICartCRUD _cartCRUD;
        private readonly ICartDetailCRUD _cartDetailCRUD;
        private readonly IVoucherCRUD _voucherCRUD;
        static private OrderVM _orderVM = new OrderVM();
        CustomerDialogVM _customerDialogVM = new CustomerDialogVM();


        public OrderController(ILogger<OrderController> logger, IOrderCRUD orderCRUD,
            IApplicationUserCRUD applicationUser, RoleManager<IdentityRole> roleManager,
             UserManager<ApplicationUser> usermanager, IAddressCRUD addressCRUD, IProductCRUD productCRUD,
             IOrderDetailCRUD orderDetailCRUD, ISizeDetailCRUD sizeDetailCRUD, ICartCRUD cartCRUD, ICartDetailCRUD cartDetailCRUD, IVoucherCRUD voucherCRUD)
        {
            _logger = logger;
            _orderCRUD = orderCRUD;
            _applicationuserCRUD = applicationUser;
            _roleManager = roleManager;
            _usermanager = usermanager;
            
            _addressCRUD = addressCRUD;
            _productCRUD = productCRUD;
            _orderDetailCRUD = orderDetailCRUD;
            _sizeDetailCRUD = sizeDetailCRUD;
            _cartCRUD = cartCRUD;
            _cartDetailCRUD = cartDetailCRUD;
            _voucherCRUD = voucherCRUD;

            Init();
        }

        private void Init()
        {

            var obj = _applicationuserCRUD.GetAllAsync().Result;

            _orderVM.customers.Clear();
            _orderVM.allOrders.Clear();

            foreach (ApplicationUser i in obj)
            {

                foreach (Order o in _orderCRUD.GetAllAsync(i.Id).Result)
                {
                    _orderVM.allOrders.Add(o);
                }

                var role = _usermanager.GetRolesAsync(i).Result.ToList();
                if (role != null && role.Count!=0 && role[0] == "Customer")
                {
                    _orderVM.customers.Add(i);
                }
            }

        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Order = true;

            var orderList = await _orderCRUD.GetAllOrderAsync();
            foreach (var item in orderList)
            {
                item.OrderDetails = await _orderDetailCRUD.GetAllAsync(item.OrderId);
                foreach (var itemDetail in item.OrderDetails)
                {
                    item.OrderTotalPayment += itemDetail.Payment;
                }
            }
            ViewData["orders"] = orderList;

            return View();
        }


        public IActionResult MakeAnOrder()
        {
            List<Voucher>? vouchers = _voucherCRUD.GetAllAsync().Result;

            ViewData["vouchers"] = vouchers;
            ViewData["deliveryMethods"] = Enum.GetValues(typeof(DeliveryMethods)).Cast<DeliveryMethods>().ToList();


            // create fake order
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
            _orderVM.currOrder.OrderDetails.Clear();

            foreach (var item in list)
            {
                orderDetail = new OrderDetail()
                {
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

        [HttpGet]
        public async Task<IActionResult> ProductDialog()
        {

            var obj = await _productCRUD.GetAllAsync();
            ViewData["products"] = obj;
            return PartialView(new List<String> { "s", "2" });
        }

        [HttpGet]
        public async Task<IActionResult> PickItem()
        {

            _orderVM.pickingQuantity.Clear();
            //ViewData["customer"] = _orderVM.customers;
            var obj = await _productCRUD.GetAllAsync();
            //ViewData["products"] = obj;
            _orderVM.products = obj;
            return View(_orderVM);
        }
        [HttpPost]
        public IActionResult PickCustomer(OrderVM orderVM)
        {
            _orderVM.pickitems = orderVM.pickitems;
            foreach (string i in orderVM.pickingQuantity)
                if (i != "0")
                    _orderVM.pickingQuantity.Add(i);
            foreach (string i in orderVM.pickingSize)
                if (i != "--")
                    _orderVM.pickingSize.Add(i);
            //_orderVM.pickingSize = orderVM.pickingSize;
            _orderVM.products.Clear();


            foreach (var item in _orderVM.pickitems)
            {
                if (item == null)
                {
                    continue;
                }
                var product = _productCRUD.GetByIdAsync(item).Result;
                product.Sizes = _sizeDetailCRUD.GetAllByIdAsync(product.ProductId).Result;
                //check size avaiables before adding
                _orderVM.products.Add(product);
            }
            _customerDialogVM.customers = _orderVM.customers;
            return View(_customerDialogVM);
        }

        public IActionResult PickCustomer()
        {
            CustomerDialogVM cusVM = new CustomerDialogVM();
            cusVM.customers = (List<ApplicationUser>?)_usermanager.GetUsersInRoleAsync(role).Result;

            _customerDialogVM = cusVM;

            return View(_customerDialogVM);
        }

        public IActionResult Edit()
        {
            //ViewBag.Order = true;
            return View();
        }

        public async Task<IActionResult> PickUser(string id)
        {
            ViewData["customer"] = _orderVM.customers;
            var obj = await _applicationuserCRUD.GetByIdAsync(id);
            var addressList = await _addressCRUD.GetAllAsync(id);
            obj.Addresses = addressList;
            ViewData["pickedUser"] = obj;
            if (obj != null) // xu ly admin se k xoa acc 
            {

                return View("Create");

            }
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(OrderVM orderVM)
        {

            if (orderVM == null || orderVM.isOnProcessing != true)
            {
                return RedirectToAction("Index");
            }
            await _orderCRUD.CreateAsync(_orderVM.currOrder);

            foreach (var s in _orderVM.currOrder.OrderDetails)
            {
                await _orderDetailCRUD.CreateAsync(s);
            }
            foreach (var s in _orderVM.products)
            {
                _productCRUD.Update(s);
            }



            var orderList = await _orderCRUD.GetAllOrderAsync();
            foreach (var item in orderList)
            {
                item.OrderDetails = await _orderDetailCRUD.GetAllAsync(item.OrderId);
                foreach (var itemDetail in item.OrderDetails)
                {
                    item.OrderTotalPayment += itemDetail.Payment;
                }
            }
            ViewData["orders"] = orderList;

            _orderVM = new OrderVM(); // cần clear lại VM

            // Clear the admin's cart
            foreach (CartDetail i in _cartDetailCRUD.GetAllAsync(_cartCRUD.GetAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result.CartId).Result)
            {
                _cartDetailCRUD.Remove(i);
            }

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmOrderAsync(CustomerDialogVM? id)
        {
            var obj = _applicationuserCRUD.GetByIdAsync(id.pickCustomerId).Result;
            
            //Handle if user isn't existed
            if (obj == null)
            {
                ApplicationUser newuser = new ApplicationUser();
                newuser.UserName = id.pickCustomers.UserName;
                newuser.Email = id.pickCustomers.Email;
                newuser.SingleAddress = id.pickCustomers.SingleAddress;
                newuser.PhoneNumber = id.pickCustomers.PhoneNumber;

                _cartCRUD.CreateAsync(new Cart() { UserId = newuser.Id });
                _addressCRUD.CreateAsync(new Address() { AddressDetail = newuser.SingleAddress, UserId = newuser.Id });
                _applicationuserCRUD.CreateAsync(newuser).Wait();

                _usermanager.AddToRoleAsync(newuser, "Passenger").Wait();

                obj = newuser;
            }

            _orderVM.customers.Clear();
            _orderVM.customers.Add(obj);
            _orderVM.customers[0].Addresses = _addressCRUD.GetAllAsync(_orderVM.customers[0].Id).Result;

            _orderVM.totalPayment = _orderVM.totalAmount = 0;

            

            if (_orderVM.currOrder == null) // Handle when admin creates order directly from cart
            {
                _orderVM.currOrder = new Order();

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                List<CartDetail> cartDetails = _cartDetailCRUD.GetAllAsync(_cartCRUD.GetAsync(userId).Result.CartId).Result;

                foreach (CartDetail cd in cartDetails)
                {
                    _orderVM.totalPayment += cd.CartDetailTotalSum;
                    _orderVM.totalAmount += cd.Amount;

                    var newOrderDetail = new OrderDetail()
                    {
                        OrderId = _orderVM.currOrder.OrderId,
                        Amount = cd.Amount,
                        Payment = cd.CartDetailTotalSum,
                        ProductId = cd.ProductId,
                        Size = cd.Size
                    };

                    _orderVM.currOrder.OrderDetails.Add(newOrderDetail);
                }
            }
            else
            {
                for (var i = 0; i < _orderVM.products.Count; i++)
                {
                    //calculate totalprice
                    var m = Int32.Parse(_orderVM.pickingQuantity[i]);
                    _orderVM.totalPayment += _orderVM.products[i].ProductUnitPrice * m;//sai nhas
                    _orderVM.totalAmount += m;//van sai nha
                                              //reduce product
                    if (m > _orderVM.products[i].Amount)
                    {
                        return NoContent();// nay can validation
                    }
                    else
                    {
                        _orderVM.products[i].Amount -= m;
                    }
                    //create detail


                    var it = new OrderDetail()
                    {
                        OrderId = _orderVM.currOrder.OrderId,
                        Amount = m,
                        Payment = _orderVM.products[i].ProductUnitPrice * m,
                        ProductId = _orderVM.products[i].ProductId
                    };

                    _orderVM.currOrder.OrderDetails.Add(it);
                }
            }

            _orderVM.currOrder.UserId = _orderVM.customers[0].Id;
            _orderVM.currOrder.Status = Core.Enums.Status.Waiting;

            return View(_orderVM);
        }
    }
}
