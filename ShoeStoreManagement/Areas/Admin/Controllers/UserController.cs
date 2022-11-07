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

        public UserController(ILogger<UserController> logger, IApplicationUserCRUD applicationuserCRUD, UserManager<ApplicationUser> usermanager)
        {
            _logger = logger;
            _applicationuserCRUD = applicationuserCRUD;
            _usermanager = usermanager;
            Init();
        }
        private void Init()
        {
            applicationUsers = _applicationuserCRUD.GetAllAsync().Result;

            foreach (ApplicationUser i in applicationUsers) {
                 var roles =  _usermanager.GetRolesAsync(i).Result.ToList();
                applicationuserRoles = roles;
            }
            
        }

        public IActionResult Index()
        {
            ViewBag.ApplicationUser = true;
            ViewData["applicationUser"] = applicationUsers;
            ViewData["userRoles"] = applicationuserRoles;
            return View();
        }

        public IActionResult Create()
        {
            //ViewBag.Product = true;
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
