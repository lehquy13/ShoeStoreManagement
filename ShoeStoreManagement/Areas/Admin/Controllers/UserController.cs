using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using Microsoft.Extensions.Hosting;
using ShoeStoreManagement.Core.ViewModel;
using ShoeStoreManagement.Views.Shared.Components;


namespace ShoeStoreManagement.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class UserController : Controller
	{
		private readonly ILogger<UserController> _logger;
		private readonly IWebHostEnvironment _hostEnvironment;
		private readonly IApplicationUserCRUD _applicationuserCRUD;
		private readonly UserManager<ApplicationUser> _usermanager;
		private readonly RoleManager<IdentityRole> _rolemanager;
		private readonly IAddressCRUD _addressCRUD;
		private readonly ICartCRUD _cartCRUD;
		private static UserVM _userVM = new UserVM();

		public UserController(ILogger<UserController> logger, IApplicationUserCRUD applicationuserCRUD, UserManager<ApplicationUser> usermanager,
			RoleManager<IdentityRole> roleManager, IAddressCRUD addressCRUD, ICartCRUD cartCRUD, IWebHostEnvironment hostEnvironment)
		{
			_logger = logger;
			_applicationuserCRUD = applicationuserCRUD;
			_usermanager = usermanager;
			_rolemanager = roleManager;
			_addressCRUD = addressCRUD;
			_cartCRUD = cartCRUD;
			_hostEnvironment = hostEnvironment;

			Init();

		}
		private void Init()
		{
			// Get All Users
			_userVM.applicationUsers = _applicationuserCRUD.GetAllAsync().Result;

			// Get User's Role
			_userVM.applicationuserRoles = new List<string>();
			_userVM.roles = new List<IdentityRole>();


			// Get All Roles
			_userVM.roles = _rolemanager.Roles.ToList();
		}

		public IActionResult Index(string filter)
		{
			ViewBag.User = true;

			if (string.IsNullOrEmpty(filter))
				filter = "All";

			List<ApplicationUser> users = new List<ApplicationUser>();
			//filter
			foreach (ApplicationUser i in _userVM.applicationUsers)
			{
				var role = _usermanager.GetRolesAsync(i).Result.ToList();

				if (role != null && role.Count != 0)
				{
					//string role = "a";
					if (!string.IsNullOrEmpty(role[0]))
					{
						i.Role = role[0];
						if (!filter.Equals("All"))
						{
							if (role.Equals(filter))
							{
								users.Add(i);
								_userVM.applicationuserRoles.Add(role[0]);
							}
						}
						else
						{
							users.Add(i);
							_userVM.applicationuserRoles.Add(role[0]);
						}
					}
				}
			}
			users = users.OrderBy(u => u.CreatedDate).ToList();

			_userVM.applicationUsers = users;

			return View(_userVM);
		}

		[HttpGet]
		public IActionResult Create()
		{
			_userVM.user = new ApplicationUser();

			return PartialView(_userVM);
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> Create(UserVM userVM)
		{
			var obj = userVM.user;

			if (obj.Avatar == null)
			{
				using (var stream = new MemoryStream())
				{
					obj.Avatar = new FormFile(stream, 0, 0, "name", "fileName");
				}
			}

			// Haven't done with user creating conditions
			ModelState.Clear();
			if (TryValidateModel(obj))
			{
				
				// Add image
				if (obj.Avatar.Length > 0)
				{
					string wwwRootPath = _hostEnvironment.WebRootPath;
					string fileName = Path.GetFileNameWithoutExtension(obj.Avatar.FileName);
					string extension = Path.GetExtension(obj.Avatar.FileName);
					fileName = fileName + extension;

					string path = Path.Combine(wwwRootPath + "/Image/", fileName);
					using (var fileStream = new FileStream(path, FileMode.Create))
					{
						await obj.Avatar.CopyToAsync(fileStream);
					}

					obj.AvatarName = fileName;

				}
				else
				{
					Random rnd = new Random();

					var num = rnd.Next(1, 3);

					string? avtName = num switch
					{
						1 => "avt1.png",
						2 => "avt2.png",
						3 => "avt3.png",
						_ => "avt1.png",
					};

					obj.AvatarName = avtName;
				}

				//await _applicationuserCRUD.CreateAsync(obj);
				var result = await _usermanager.CreateAsync(obj, "Password0*!");
				await _cartCRUD.CreateAsync(new Cart() { UserId = obj.Id });
				await _addressCRUD.CreateAsync(new Address() { AddressDetail = obj.SingleAddress, UserId = obj.Id });

				_usermanager.AddToRoleAsync(obj, obj.Role).Wait();
				var users = _applicationuserCRUD.GetAllAsync().Result;

				foreach (ApplicationUser i in users)
				{
					var role = _usermanager.GetRolesAsync(i).Result.ToList();

					if (role != null && role.Count != 0)
					{
						if (!string.IsNullOrEmpty(role[0]))
						{
							i.Role = role[0];
							users.Add(i);

						}
					}
				}

				users = users.OrderBy(u => u.CreatedDate).ToList();
				_userVM.applicationUsers = users;

				return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _userVM.applicationUsers) });
			}
			return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", _userVM) });
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string? id)
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

				_userVM.user = obj;

				return PartialView(_userVM);
			}
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		public IActionResult Edit(UserVM userVM)
		{
			var obj = userVM.user;

			if (obj.Avatar == null)
			{
				using (var stream = new MemoryStream())
				{
					obj.Avatar = new FormFile(stream, 0, 0, "name", "fileName");
				}
			}

			ModelState.Clear();
			if (TryValidateModel(obj))
			{
				if (obj.Avatar.Length > 0)
				{
					string wwwRootPath = _hostEnvironment.WebRootPath;
					string fileName = Path.GetFileNameWithoutExtension(obj.Avatar.FileName);
					string extension = Path.GetExtension(obj.Avatar.FileName);
					fileName = fileName + extension;

					string path = Path.Combine(wwwRootPath + "/Image/", fileName);
					using (var fileStream = new FileStream(path, FileMode.Create))
					{
						obj.Avatar.CopyToAsync(fileStream);
					}

					obj.AvatarName = fileName;
				}
				else
				{
					obj.AvatarName = _userVM.user.AvatarName;
				}

				_applicationuserCRUD.Update(obj);

				var users = _applicationuserCRUD.GetAllAsync().Result;

				users = users.OrderBy(u => u.CreatedDate).ToList();
				_userVM.applicationUsers = users;

				return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _userVM.applicationUsers) });
			}
			return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", _userVM) });
		}

		[HttpPost]
		public IActionResult Delete(string id)
		{
			var obj = _applicationuserCRUD.GetByIdAsync(id).Result;
			if (obj != null) // xu ly admin se k xoa acc 
			{
				if (obj.Role != "Admin")
				{
					_addressCRUD.DeleteAllAdressByIdAsync(obj.Id);
					_applicationuserCRUD.Remove(obj);
				}
				var users = _applicationuserCRUD.GetAllAsync().Result;
				users = users.OrderBy(u => u.CreatedDate).ToList();
				_userVM.applicationUsers = users;
			}
			return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _userVM.applicationUsers) });
		}
	}
}
