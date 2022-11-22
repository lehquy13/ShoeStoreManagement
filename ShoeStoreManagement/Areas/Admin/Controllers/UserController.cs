using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;


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
            ViewBag.User = true;

            if (string.IsNullOrEmpty(filter))
                filter = "All";

            List<ApplicationUser> users = new List<ApplicationUser>();

            foreach (ApplicationUser i in applicationUsers)
            {
				var role = _usermanager.GetRolesAsync(i).Result.ToList();
                if (role != null && role.Count != 0 )
                {
                    //string role = "a";
                    if (!string.IsNullOrEmpty(role[0]))
                    {
                        if (!filter.Equals("All"))
                        {
                            if (role.Equals(filter))
                            {
                                users.Add(i);
                                applicationuserRoles.Add(role[0]);
                            }
                        }
                        else
                        {
                            users.Add(i);
                            applicationuserRoles.Add(role[0]);
                        }
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
            if (ModelState.IsValid && obj.Role != string.Empty)
            {
                _cartCRUD.CreateAsync(new Cart() { UserId = obj.Id });
                _addressCRUD.CreateAsync(new Address() { AddressDetail = obj.SingleAddress, UserId = obj.Id });
                _applicationuserCRUD.CreateAsync(obj);

                _usermanager.AddToRoleAsync(obj, obj.Role).Wait();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || id == "")
            {
                return NotFound();
            }
            else
            {
                var obj = await _applicationuserCRUD.GetByIdAsync(id);
                if (obj == null)
                {
                    return NotFound();
                }
                obj.Role = _usermanager.GetRolesAsync(obj).Result.ToList()[0];
                ViewData["userRoles"] = roles;
                return PartialView(obj);
            }
        }

        [HttpPost]
        public IActionResult Edit(ApplicationUser obj)
        {
            if (ModelState.IsValid)
            {
                _applicationuserCRUD.Update(obj);

                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public async Task<IActionResult> Delete(string id)
        {
            var obj = await _applicationuserCRUD.GetByIdAsync(id);
            if (obj != null) // xu ly admin se k xoa acc 
            {
                obj.Role = _usermanager.GetRolesAsync(obj).Result.ToList()[0];
                if (obj.Role != "Admin")
                {
                    _addressCRUD.DeleteAllAdressByIdAsync(obj.Id);
                    _applicationuserCRUD.Remove(obj);
                }

            }
            return RedirectToAction("Index");
        }
    }
}
