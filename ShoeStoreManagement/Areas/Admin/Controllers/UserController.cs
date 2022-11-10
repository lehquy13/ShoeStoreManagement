using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Controllers;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using ShoeStoreManagement.Views.Shared.Components;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IApplicationUserCRUD _applicationuserCRUD;
        private readonly UserManager<ApplicationUser> _usermanager;
        private List<ApplicationUser>? applicationUsers;
        private List<string>? applicationuserRoles;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private List<IdentityRole>? roles;
        private readonly IAddressCRUD _addressCRUD;
        private readonly ICartCRUD _cartCRUD;

        public UserController(ILogger<UserController> logger, IApplicationUserCRUD applicationuserCRUD, UserManager<ApplicationUser> usermanager,
            RoleManager<IdentityRole> roleManager, IAddressCRUD addressCRUD, ICartCRUD cartCRUD)
        {
            _logger = logger;
            _applicationuserCRUD = applicationuserCRUD;
            _usermanager = usermanager;
            _rolemanager = roleManager;
            _addressCRUD = addressCRUD;
            _cartCRUD = cartCRUD;

            Init();
        }
        private void Init()
        {
            // Get All Users
            applicationUsers = _applicationuserCRUD.GetAllAsync().Result;

            // Get User's Role
            applicationuserRoles = new List<string>();
            roles = new List<IdentityRole>();


            // Get All Roles
            roles = _rolemanager.Roles.ToList();
        }

        public IActionResult Index(string filter)
        {
            ViewBag.ApplicationUser = true;

            if (string.IsNullOrEmpty(filter))
                filter = "All";

            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (ApplicationUser i in applicationUsers)
            {
                string role = _usermanager.GetRolesAsync(i).Result.ToList()[0];
                //string role = "a";
                if (!string.IsNullOrEmpty(role))
                {
                    if (!filter.Equals("All"))
                    {
                        if (role.Equals(filter))
                        {
                            users.Add(i);
                            applicationuserRoles.Add(role);
                        }
                    }
                    else
                    {
                        users.Add(i);
                        applicationuserRoles.Add(role);
                    }
                }
            }

            ViewData["applicationUser"] = users;
            ViewData["userRoles"] = applicationuserRoles;
            ViewData["allRoles"] = roles;
            return View();
        }

        [HttpPost]
        public IActionResult Create(ApplicationUser obj)
        {
            // Haven't done with user creating conditions

            if (ModelState.IsValid && obj.selectedRole != string.Empty)
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
            return View();
        }
    }
}
