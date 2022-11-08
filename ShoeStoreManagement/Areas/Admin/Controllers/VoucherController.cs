using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Enums;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Data;
using ValueType = ShoeStoreManagement.Core.Enums.ValueType;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VoucherController : Controller
	{
        private readonly IVoucherCRUD _voucherCRUD;

        public VoucherController(IVoucherCRUD voucherCRUD)
        {
            _voucherCRUD = voucherCRUD;
        }
        
        public IActionResult Index()
		{
            ViewBag.Voucher = true;

            ViewData["vouchers"] = _voucherCRUD.GetAllAsync().Result;
            ViewData["conditionTypes"] = Enum.GetValues(typeof(ConditionType)).Cast<ConditionType>().ToList();
            ViewData["valueTypes"] = Enum.GetValues(typeof(ValueType)).Cast<ValueType>().ToList();
            ViewData["expiredTypes"] = Enum.GetValues(typeof(ExpireType)).Cast<ExpireType>().ToList();

            return View();
		}

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Voucher obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.CreatedDate > DateTime.Now)
                {
                    obj.State = VoucherStatus.ComingSoon;
                }

                if (obj.ExpiredType == ExpireType.ExpiredDate)
                {
                    // check if status is expired

                    //
                }

                if (obj.ValueType == ValueType.Percent && obj.Value > 100)
                {
                    // validate
                }

                if (obj.ConditionType == ConditionType.NewCustomer)
                {
                    obj.ConditionValue = "";
                }

                _voucherCRUD.CreateAsync(obj);
                return RedirectToAction("Index");
            }

            return View(obj);
        }
    }
}
