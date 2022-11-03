using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        private List<ApplicationUser>? applicationUsers;

        private void Init()
        {
            applicationUsers = _applicationuserCRUD.GetAllAsync().Result;
        }

        public IActionResult Index()
        {
            ViewBag.ApplicationUser = true;
            ViewData["applicationUser"] = applicationUsers;
            return View();
        }

        public IActionResult Create()
        {
            //ViewBag.Product = true;
            return View();
        }
    }
}
