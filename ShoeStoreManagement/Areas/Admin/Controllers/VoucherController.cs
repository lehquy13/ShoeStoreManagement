using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;
using System.Data;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
	public class VoucherController : Controller
	{
        private readonly IVoucherCRUD _voucherCRUD;

        public VoucherController(IVoucherCRUD voucherCRUD)
        {
            _voucherCRUD = voucherCRUD;
        }

        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
		{
            ViewBag.Voucher = true;

            ViewData["vouchers"] = _voucherCRUD.GetAllAsync().Result;

            return View();
		}

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
