using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Enums;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Implementations;
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

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || id == "")
            {
                return NotFound();
            }
            var obj = await _voucherCRUD.GetByIdAsync(id);
            ViewData["vouchers"] = _voucherCRUD.GetAllAsync().Result;
            ViewData["conditionTypes"] = Enum.GetValues(typeof(ConditionType)).Cast<ConditionType>().ToList();
            ViewData["valueTypes"] = Enum.GetValues(typeof(ValueType)).Cast<ValueType>().ToList();
            ViewData["expiredTypes"] = Enum.GetValues(typeof(ExpireType)).Cast<ExpireType>().ToList();

            return View(obj);
        }

        [HttpPost]
        public IActionResult Edit(Voucher obj)
        {
            if (ModelState.IsValid)
            {
                //var temp = obj.TestSizeAmount.Where(x => x != "0").ToList();

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
                _voucherCRUD.Update(obj);


                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var obj = await _voucherCRUD.GetByIdAsync(id);
            if (obj != null)
            {
                _voucherCRUD.Remove(obj);
            }
            return RedirectToAction("Index");
        }
    }
}
