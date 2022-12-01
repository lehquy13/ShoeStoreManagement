// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core;
using ShoeStoreManagement.Data;

namespace ShoeStoreManagement.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = applicationDbContext;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [RegularExpression(@"^([\+]?84[-]?|[0])?[1-9][0-9]{8}$", ErrorMessage = "Invalid Phone Numbber!")]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [MinimunYear(1960)]
            [DataType(DataType.DateTime)]
            [Display(Name = "Birthday")]
            public DateTime Birthday { get; set; }

            [Display(Name = "Name")]
            public string Name { get; set; }

            [Display(Name = "Avatar")]
            public IFormFile Avatar { get; set; }

            [Display(Name = "Avatar Name")]
            public string AvatarName { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var birthday = user.Birthday;
            var avatar = user.Avatar;
            var avatarName = user.AvatarName;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Birthday = birthday,
                Name = userName,
                Avatar = avatar,
                AvatarName = avatarName,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            var birthday = user.Birthday;
            if (Input.Birthday != birthday)
            {
                user.Birthday = Input.Birthday;
                _context.ApplicationUsers.Update(user);
                _context.SaveChanges();

                StatusMessage = "Update birthdaySuccessful";
                return RedirectToPage();
            }


            if (Input.Avatar == null)
            {
                using (var stream = new MemoryStream())
                {
                    Input.Avatar = new FormFile(stream, 0, 0, "name", "fileName");
                }
            }

            string fileName = "";
            string wwwRootPath = _hostEnvironment.WebRootPath;

            if (Input.Avatar.Length > 0)
            {
                fileName = Path.GetFileNameWithoutExtension(Input.Avatar.FileName);
                string extension = Path.GetExtension(Input.Avatar.FileName);
                fileName = fileName + extension;
            }

            if (user.AvatarName != fileName)
            {
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await Input.Avatar.CopyToAsync(fileStream);
                }

                user.AvatarName = fileName;

                _context.ApplicationUsers.Update(user);
                _context.SaveChanges();

                StatusMessage = "Succesful.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
