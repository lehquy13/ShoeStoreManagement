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


        static private readonly OrderVM _orderVM = new OrderVM();
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

            foreach (ApplicationUser i in obj)
            {

                foreach (Order o in _orderCRUD.GetAllAsync(i.Id).Result)
                {
                    _orderVM.orders.Add(o);
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
        //k xai
        [HttpPost]
        public async Task<IActionResult> Index(OrderVM orderVM)
        {

            if (orderVM == null)
            {
                return RedirectToAction("Index");
            }

            var order = new Order();
            order.UserId = _orderVM.customers[0].Id;
            order.Status = Core.Enums.Status.sampleStatus;
            await _orderCRUD.CreateAsync(order);

            foreach (var s in _orderVM.products)
            {
                var it = new OrderDetail()
                {
                    OrderId = order.OrderId,
                    Amount = _orderVM.totalAmount,
                    Payment = _orderVM.totalPayment,
                    ProductId = s.ProductId
                };

                await _orderDetailCRUD.CreateAsync(it);
                _productCRUD.Update(s);
            }

            var orderList = await _orderCRUD.GetAllOrderAsync();
            foreach(var item in orderList)
            {
                item.OrderDetails = await _orderDetailCRUD.GetAllAsync(item.OrderId);
                foreach(var itemDetail in item.OrderDetails)
                {
                    item.OrderTotalPayment += itemDetail.Payment;
                }
            }
            ViewData["orders"] = orderList;

            return View();


        }

        [HttpPost]
        public IActionResult ConfirmOrder(CustomerDialogVM id)
        {
            Console.WriteLine("me here");
            if (id.pickCustomerId == null)
            {
                return RedirectToAction("Index");
            }

            _orderVM.customers[0].Addresses = _addressCRUD.GetAllAsync(_orderVM.customers[0].Id).Result;
            var obj = _applicationuserCRUD.GetByIdAsync(id.pickCustomerId).Result;
            _orderVM.customers.Clear();
            _orderVM.customers.Add(obj);
            _orderVM.totalPayment = _orderVM.totalAmount = 0;

            for (var i = 0; i < _orderVM.products.Count; i++)
            {
                var m = Int32.Parse(_orderVM.pickingQuantity[i]);
                _orderVM.totalPayment += _orderVM.products[i].ProductUnitPrice * m;//sai nhas
                _orderVM.totalAmount += m;//van sai nha
                if (m > _orderVM.products[i].Amount)
                {
                    return NoContent();// nay can validation
                }
                else
                {
                    _orderVM.products[i].Amount -= m;
                }
            }

            return View(_orderVM);
        }

        
    }
}
