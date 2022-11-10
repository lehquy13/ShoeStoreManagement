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
            applicationUsers = _applicationuserCRUD.GetAllAsync().Result;
            applicationuserRoles = new List<string>();
            roles = new List<IdentityRole>();

            foreach (ApplicationUser i in applicationUsers)
            {
                string role = _usermanager.GetRolesAsync(i).Result.ToList()[0];
                //string role = "a";
                if (!string.IsNullOrEmpty(role))
                {
                    applicationuserRoles.Add(role);
                }
            }

            roles = _rolemanager.Roles.ToList();
        }

        public IActionResult Index()
        {
            ViewBag.ApplicationUser = true;
            ViewData["applicationUser"] = applicationUsers;
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
                obj.selectedRole = _usermanager.GetRolesAsync(obj).Result.ToList()[0];
                ViewData["userRoles"] = roles;
                return View(obj);
            }
        }

        [HttpPost]
        public IActionResult Edit(ApplicationUser obj)
        {
            if (ModelState.IsValid)
            {
                //var temp = obj.TestSizeAmount.Where(x => x != "0").ToList();

                //need to update adress

                //for (var i = 35; i <= 44; i++)
                //{
                //    var tempDetail = _sizeDetailCRUD.GetProductSizeAsync(obj.ProductId, i).Result;
                //    int amount = Int32.Parse(obj.TestSizeAmount[i - 35]);
                //    var newDetail = new SizeDetail() { Amount = amount, Size = i, ProductId = obj.ProductId };

                //    if (tempDetail != null)
                //    {
                //        if (amount > 0 && amount != tempDetail.Amount)
                //        {
                //            _sizeDetailCRUD.Update(newDetail);

                //        }
                //        else if (amount == 0 || !obj.TestSize.Contains(i.ToString()))
                //        {
                //            _sizeDetailCRUD.Remove(newDetail);
                //        }

                //    }
                //    else
                //    {
                //        if (amount > 0)
                //            _sizeDetailCRUD.CreateAsync(new SizeDetail()
                //            {
                //                Size = i,
                //                Amount = amount,
                //                ProductId = obj.ProductId
                //            });
                //    }


                //}
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
                obj.selectedRole = _usermanager.GetRolesAsync(obj).Result.ToList()[0];
                if (obj.selectedRole != "Admin")
                {
                    _addressCRUD.DeleteAllAdressByIdAsync(obj.Id);
                    _applicationuserCRUD.Remove(obj);
                }

            }
            return RedirectToAction("Index");
        }
    }
}
