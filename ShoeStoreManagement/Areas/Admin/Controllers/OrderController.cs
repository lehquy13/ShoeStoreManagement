using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Data;

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

        public OrderController(ILogger<OrderController> logger, IOrderCRUD orderCRUD, IApplicationUserCRUD applicationUser)
        {
            _logger = logger;
            _orderCRUD = orderCRUD;
            _applicationuserCRUD = applicationUser;

            Init();
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
            }
        }
        public IActionResult Index()
        {
            ViewBag.Order = true;
            ViewData["orders"] = orders;

            return View();
        }

        public IActionResult Create()
        {
            if (ModelState.IsValid)
            {
                _cartCRUD.CreateAsync(new Cart() { UserId = obj.Id });
                _addressCRUD.CreateAsync(new Address() { AddressDetail = obj.singleAddress, UserId = obj.Id });
                _applicationuserCRUD.CreateAsync(obj);

                _usermanager.AddToRoleAsync(obj, obj.selectedRole).Wait();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit()
        {
            //ViewBag.Order = true;
            return View();
        }
      

    }
}
