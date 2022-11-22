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
        List<Product> productList;


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
                if (role != null && role.Count != 0 && role[0] == "Customer")
                {
                    _orderVM.customers.Add(i);
                }
            }

            productList = _productCRUD.GetAllAsync().Result;

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
        public IActionResult PickItemDialog()
        {
            var obj = _productCRUD.GetAllAsync().Result;

            if (obj.Count > 0)
            {
                foreach (var i in obj)
                {
                    i.Sizes = _sizeDetailCRUD.GetAllByIdAsync(i.ProductId).Result;

                }
            }
            ViewData["products"] = obj;
            _orderVM.pickitems.Clear();
            _orderVM.pickingQuantity.Clear();
            _orderVM.pickingSize.Clear();
            return PartialView(_orderVM);
        }

        // Quy dang lam o day
        [HttpPost]
        public IActionResult PickedItem(OrderVM orderVM)
        {
            _orderVM.pickitems = orderVM.pickitems;
            var index = 0;

            for (int i = 0; i < orderVM.pickingQuantity.Count; i++)
            {
                if (orderVM.pickingQuantity[i] != "0" && orderVM.pickingSize[i] != "--")
                {
                    _orderVM.pickingQuantity.Add(orderVM.pickingQuantity[i]);
                    _orderVM.pickingSize.Add(orderVM.pickingSize[i]);
                    _orderVM.pickitems.Add(productList[i].ProductId);

                    Product product = productList[i];
                    product.Sizes.Add(new SizeDetail()
                    {
                        Amount = Int32.Parse(_orderVM.pickingQuantity[index]),
                        Size = Int32.Parse(_orderVM.pickingSize[index])
                    });
                    var newOrderD = new OrderDetail() { Product = product, ProductId = product.ProductId,
                        OrderId = _orderVM.currOrder.OrderId,
                        Amount = Int32.Parse(_orderVM.pickingQuantity[index]),
                        Size = Int32.Parse(_orderVM.pickingSize[index])

                    };
                    _orderVM.currentOrderDetail.Add(newOrderD);
                    index++;
                }
            }
            _customerDialogVM.customers = _orderVM.customers;
            return RedirectToAction("PickItem");
        }

        public IActionResult DeteleItem(string id)
        {
            
                foreach (var i in _orderVM.currentOrderDetail.ToList())
                    if (id == i.OrderDetailId)
                        _orderVM.currentOrderDetail.Remove(i);

            return RedirectToAction("PickItem");
        }

        [HttpGet]
        public async Task<IActionResult> PickItem()
        {
            _orderVM.pickingQuantity.Clear();
            //ViewData["customer"] = _orderVM.customers;
            var obj = await _productCRUD.GetAllAsync();
            if (obj.Count > 0)
            {
                foreach (var i in obj)
                {
                    i.Sizes = _sizeDetailCRUD.GetAllByIdAsync(i.ProductId).Result;

                }
            }
            //_orderVM.products = obj;
            ViewData["products"] = obj;

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
            //_orderVM.products.Clear();


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

        [HttpPost]
        public IActionResult Edit()
        {
            //ViewBag.Order = true;
            return View();
        }

        //confirm Index
        [HttpPost]
        public async Task<IActionResult> Index(OrderVM orderVM)
        {

            if (orderVM == null || orderVM.isOnProcessing != true)
            {
                return RedirectToAction("Index");
            }
            foreach (var orderDetail in _orderVM.currOrder.OrderDetails)
            {
                _orderVM.currOrder.OrderTotalPayment += orderDetail.Payment;
                _orderVM.currOrder.TotalAmount += orderDetail.Amount;
                
            }
            await _orderCRUD.CreateAsync(_orderVM.currOrder);

            
            foreach (var s in _orderVM.currentOrderDetail)
            {
                var soldAmount = s.Product.Sizes[0].Amount;

                var sizeD = _sizeDetailCRUD.GetProductSizeAsync(s.ProductId, s.Product.Sizes[0].Size).Result;
                sizeD.Amount -= soldAmount;
                _sizeDetailCRUD.Update(sizeD);
                var product = s.Product;
                product.Amount -= soldAmount;
                _productCRUD.Update(product); 
            }
            foreach (var s in _orderVM.currOrder.OrderDetails)
            {
                s.Product = null;
                await _orderDetailCRUD.CreateAsync(s);
            }
            _orderVM = new OrderVM(); // cần clear lại VM
            Init();


            var orderList = await _orderCRUD.GetAllOrderAsync();
            foreach (var item in orderList)
            {
                item.OrderDetails = await _orderDetailCRUD.GetAllAsync(item.OrderId);
                //foreach (var itemDetail in item.OrderDetails)
                //{
                //    item.OrderTotalPayment += itemDetail.Payment;
                //}
            }
            ViewData["orders"] = orderList;


            // Clear the admin's cart
            foreach (CartDetail i in _cartDetailCRUD.GetAllAsync(_cartCRUD.GetAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result.CartId).Result)
            {
                _cartDetailCRUD.Remove(i);
            }

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmOrderAsync(CustomerDialogVM id)
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

            _orderVM.currOrder.OrderDetails.Clear();

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
                for (var i = 0; i < _orderVM.currentOrderDetail.Count; i++)
                {
                    //calculate totalprice
                    var product = _orderVM.currentOrderDetail[i].Product;
                    var m = product.Sizes[0].Amount;
                    _orderVM.totalPayment += product.ProductUnitPrice * m;//sai nhas
                    _orderVM.totalAmount += m;//van sai nha
                                              //reduce product
                                              //create detail


                    //var it = new OrderDetail()
                    //{
                    //    OrderId = _orderVM.currOrder.OrderId,
                    //    Amount = m,
                    //    Payment = _orderVM.products[i].ProductUnitPrice * m,
                    //    ProductId = _orderVM.products[i].ProductId
                    //};
                    _orderVM.currOrder.OrderDetails[i].Amount = _orderVM.totalAmount;
                    _orderVM.currOrder.OrderDetails[i].Payment = _orderVM.totalPayment;

                    _orderVM.currOrder.OrderDetails.Add(_orderVM.currentOrderDetail[i]);
                }
            }

            _orderVM.currOrder.UserId = _orderVM.customers[0].Id;
            _orderVM.currOrder.Status = Core.Enums.Status.Waiting;

            return View(_orderVM);
        }
    }
}
