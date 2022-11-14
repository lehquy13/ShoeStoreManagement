using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModels;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.CRUD.Interfaces;


namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        // DI
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderCRUD _orderCRUD;
        private readonly IOrderDetailCRUD _orderDetailCRUD;
        private readonly IApplicationUserCRUD _applicationuserCRUD;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IAddressCRUD _addressCRUD;
        private readonly IProductCRUD _productCRUD;

        static private OrderVM _orderVM = new OrderVM();
        CustomerDialogVM _customerDialogVM = new CustomerDialogVM();


        public OrderController(ILogger<OrderController> logger, IOrderCRUD orderCRUD,
            IApplicationUserCRUD applicationUser, RoleManager<IdentityRole> roleManager,
             UserManager<ApplicationUser> usermanager, IAddressCRUD addressCRUD, IProductCRUD productCRUD,
             IOrderDetailCRUD orderDetailCRUD)
        {
            _logger = logger;
            _orderCRUD = orderCRUD;
            _applicationuserCRUD = applicationUser;
            _roleManager = roleManager;
            _usermanager = usermanager;
            Init();
            _addressCRUD = addressCRUD;
            _productCRUD = productCRUD;
            _orderDetailCRUD = orderDetailCRUD;
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

                i.Role = _usermanager.GetRolesAsync(i).Result.ToList()[0];
                if (i.Role == "Customer")
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
            ViewData["customer"] = _orderVM.customers;
            var obj = await _productCRUD.GetAllAsync();
            ViewData["products"] = obj;
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
            _orderVM.pickingSize = orderVM.pickingSize;
            _orderVM.products.Clear();


            foreach (var item in _orderVM.pickitems)
            {
                _orderVM.products.Add(_productCRUD.GetByIdAsync(item).Result);
            }
            _customerDialogVM.customers = _orderVM.customers;
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

            _orderVM = new OrderVM();

            return View();


        }

        [HttpPost]
        public IActionResult ConfirmOrderAsync(CustomerDialogVM id)
        {
            Console.WriteLine("me here");
            if (id.pickCustomerId == null)
            {
                return RedirectToAction("Index");
            }

            var obj = _applicationuserCRUD.GetByIdAsync(id.pickCustomerId).Result;
            _orderVM.customers.Clear();
            _orderVM.customers.Add(obj);
            _orderVM.customers[0].Addresses = _addressCRUD.GetAllAsync(_orderVM.customers[0].Id).Result;

            _orderVM.totalPayment = _orderVM.totalAmount = 0;

            _orderVM.currOrder.UserId = _orderVM.customers[0].Id;
            _orderVM.currOrder.Status = Core.Enums.Status.sampleStatus;
            

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

            return View(_orderVM);
        }


    }
}
