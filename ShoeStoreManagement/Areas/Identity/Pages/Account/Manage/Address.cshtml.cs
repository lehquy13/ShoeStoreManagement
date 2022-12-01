// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.CRUD.Interfaces;

namespace ShoeStoreManagement.Areas.Identity.Pages.Account.Manage
{
    public class AddressModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAddressCRUD _addressCrud;
        private readonly ILogger<AddressModel> _logger;
        public ApplicationUser currentUser = new ApplicationUser();
        public string currentAddress = "";
        public AddressModel(
            UserManager<ApplicationUser> userManager,
            ILogger<AddressModel> logger, IAddressCRUD addressCrud)
        {
            _userManager = userManager;
            _logger = logger;
            _addressCrud = addressCrud;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            else
            {
                var o = await _addressCrud.GetAllAsync(user.Id);
                if (o != null)
                {
                    user.Addresses = o;

                }
                currentUser = user;

            }


            return Page();
        }
    }
}
