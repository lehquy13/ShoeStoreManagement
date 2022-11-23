﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Enums;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModel;
using ShoeStoreManagement.Core.ViewModels;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.CRUD.Interfaces;
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
            foreach (var p in productList)
            {
                p.Sizes = _sizeDetailCRUD.GetAllByIdAsync(p.ProductId).Result;
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
                    //product.Sizes.Add(new SizeDetail()
                    //{
                    //    Amount = Int32.Parse(_orderVM.pickingQuantity[index]),
                    //    Size = Int32.Parse(_orderVM.pickingSize[index])
                    //});
                    var newOrderD = new OrderDetail()
                    {
                        Product = product,
                        ProductId = product.ProductId,
                        OrderId = _orderVM.currOrder.OrderId,
                        Amount = Int32.Parse(_orderVM.pickingQuantity[index]),
                        Size = Int32.Parse(_orderVM.pickingSize[index]),
                        Payment = Int32.Parse(_orderVM.pickingQuantity[index]) * product.ProductUnitPrice,
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
            _customerDialogVM.customers = _orderVM.customers;
            return View(_customerDialogVM);
        } 
        public async Task<IActionResult> CompletedCheck(string id)
        {			
			var orderList = await _orderCRUD.GetAllOrderAsync();
			foreach (var item in orderList)
			{
                if(item.OrderId == id)
                {
					item.Status = Status.Finish;
                    _orderCRUD.Update(item);
				}
				item.OrderDetails = await _orderDetailCRUD.GetAllAsync(item.OrderId);
				
			}
			ViewData["orders"] = orderList;
			return RedirectToAction("Index");
        }

		public async Task<IActionResult> CanceledCheck(string id)
		{
			var orderList = await _orderCRUD.GetAllOrderAsync();
			foreach (var item in orderList)
			{
				if (item.OrderId == id)
				{
					item.Status = Status.Canceled;
					_orderCRUD.Update(item);
				}
				item.OrderDetails = await _orderDetailCRUD.GetAllAsync(item.OrderId);
				
			}
			ViewData["orders"] = orderList;
			return RedirectToAction("Index");
		}


		[HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var obj = await _orderCRUD.GetByIdAsync(id);
            if (obj != null)
            {
                obj.OrderDetails = await _orderDetailCRUD.GetAllAsync(obj.OrderId);
                
                obj.User = await _applicationuserCRUD.GetByIdAsync(obj.UserId);
                foreach(var i in obj.OrderDetails)
                {
                    i.Product = await _productCRUD.GetByIdAsync(i.ProductId);
                }

                return PartialView(obj);

            }
            return RedirectToAction("Index");
        }

        //confirm and get back to Index
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
                var soldAmount = s.Amount;

                var sizeD = _sizeDetailCRUD.GetProductSizeAsync(s.ProductId, s.Size).Result;
                if (sizeD.Amount > soldAmount)
                    sizeD.Amount -= soldAmount;
                else
                {
                    _orderCRUD.Remove(_orderVM.currOrder);
                    _orderVM = new OrderVM(); // cần clear lại VM
                    Init();
                    var orderList1 = await _orderCRUD.GetAllOrderAsync();
                    foreach (var item in orderList1)
                    {
                        item.OrderDetails = await _orderDetailCRUD.GetAllAsync(item.OrderId);
                        //foreach (var itemDetail in item.OrderDetails)
                        //{
                        //    item.OrderTotalPayment += itemDetail.Payment;
                        //}
                    }
                    ViewData["orders"] = orderList1;
                    //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                    return View();
                }
                await _sizeDetailCRUD.Update(sizeD);
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
                if (id.pickCustomers.UserName != null && id.pickCustomers.Email != null && id.pickCustomers.SingleAddress != null && id.pickCustomers.PhoneNumber != null)
                {
                    newuser.UserName = id.pickCustomers.UserName;
                    newuser.Email = id.pickCustomers.Email;
                    newuser.SingleAddress = id.pickCustomers.SingleAddress;
                    newuser.PhoneNumber = id.pickCustomers.PhoneNumber;
                }

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
                    //var product = _orderVM.currentOrderDetail[i].Product;
                    var m = _orderVM.currentOrderDetail[i].Amount;
                    _orderVM.totalPayment += _orderVM.currentOrderDetail[i].Product.ProductUnitPrice * m;//sai nhas
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


                    _orderVM.currOrder.OrderDetails.Add(_orderVM.currentOrderDetail[i]);
                    //_orderVM.currOrder.OrderDetails[i].Amount = _orderVM.totalAmount;
                    //_orderVM.currOrder.OrderDetails[i].Payment = _orderVM.totalPayment;
                }
            }

            _orderVM.currOrder.UserId = _orderVM.customers[0].Id;
            _orderVM.currOrder.Status = Core.Enums.Status.Waiting;

            return View(_orderVM);
        }
    }
}
