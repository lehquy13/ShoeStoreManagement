using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.CRUD.Interfaces;


namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderCRUD _orderCRUD;
        private readonly IApplicationUserCRUD _applicationuserCRUD;
        private List<Order>? orders;
        private List<ApplicationUser>? applicationUsers;
        private List<ApplicationUser>? customers = new List<ApplicationUser>();
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly IAddressCRUD _addressCRUD;
        private readonly IProductCRUD _productCRUD;

        public OrderController(ILogger<OrderController> logger, IOrderCRUD orderCRUD,
            IApplicationUserCRUD applicationUser, RoleManager<IdentityRole> roleManager,
             UserManager<ApplicationUser> usermanager, IAddressCRUD addressCRUD, IProductCRUD productCRUD)
        {
            _logger = logger;
            _orderCRUD = orderCRUD;
            _applicationuserCRUD = applicationUser;
            _roleManager = roleManager;
            _usermanager = usermanager;
            Init();
            _addressCRUD = addressCRUD;
            _productCRUD = productCRUD;
        }

        private void Init()
        {
            applicationUsers = _applicationuserCRUD.GetAllAsync().Result;
            orders = new List<Order>();

            foreach (ApplicationUser i in applicationUsers)
            {
                foreach (Order o in _orderCRUD.GetAllAsync(i.Id).Result)
                {
                    orders.Add(o);
                }

                i.Role = _usermanager.GetRolesAsync(i).Result.ToList()[0];
                if (i.Role == "Customer")
                {
                    customers.Add(i);
                }
            }


        }
        public IActionResult Index()
        {
            ViewBag.Order = true;
            ViewData["orders"] = orders;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {

            var obj = await _productCRUD.GetAllAsync();
            ViewData["products"] = obj;


            ViewData["customer"] = customers;



            return View();
        }

        public IActionResult Edit()
        {
            //ViewBag.Order = true;
            return View();
        }

        public async Task<IActionResult> PickUser(string id)
        {
            ViewData["customer"] = customers;
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

        public async Task<IActionResult> PickItems(List<string> strings)
        {
            if(strings == null)
            {
                return RedirectToAction("Index");
            }
            var list = new List<Product>();
            foreach (string s in strings)
            {
                var it = await _productCRUD.GetByIdAsync(s);
                if (it != null)
                    list.Add(it);
            }
            ViewData["pickedItems"] = list;
            return View("Create");


        }

    }
}
