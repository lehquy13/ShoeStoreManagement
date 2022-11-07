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
        private List<string>? roles;


        public UserController(ILogger<UserController> logger, IApplicationUserCRUD applicationuserCRUD, UserManager<ApplicationUser> usermanager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _applicationuserCRUD = applicationuserCRUD;
            _usermanager = usermanager;
            _rolemanager = roleManager;
            Init();
        }
        private void Init()
        {
            applicationUsers = _applicationuserCRUD.GetAllAsync().Result;

            foreach (ApplicationUser i in applicationUsers) {
                var role = _usermanager.GetRolesAsync(i).Result.ToList()[0];

                if (role is not null) {
                    applicationuserRoles.Add(role);
                }
            }
            
            var roleList = _rolemanager.Roles.ToList();
            foreach (IdentityRole i in roleList)
            {
                roles.Add(i.Name);
            }
        }

        public IActionResult Index()
        {
            ViewBag.ApplicationUser = true;
            ViewData["applicationUser"] = applicationUsers;
            ViewData["userRoles"] = applicationuserRoles;
            ViewData["allRoles"] = roles;
            return View();
        }

        public IActionResult Create(ApplicationUser obj)
        {
            // Haven't done with user creating conditions

            if (ModelState.IsValid)
            {
                _applicationuserCRUD.CreateAsync(obj);

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
