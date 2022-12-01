﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Enums;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModel;
using ShoeStoreManagement.CRUD.Interfaces;
using ValueType = ShoeStoreManagement.Core.Enums.ValueType;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VoucherController : Controller
    {
        private readonly IVoucherCRUD _voucherCRUD;
        private static VoucherVM? _voucherVM = new VoucherVM();

        public VoucherController(IVoucherCRUD voucherCRUD)
        {
            _voucherCRUD = voucherCRUD;

            _voucherVM.vouchers = _voucherCRUD.GetAllAsync().Result;
            foreach (var v in _voucherVM.vouchers)
            {
                if (v.ExpiredValue == "0")
                {
                    v.State = VoucherStatus.Expired;
                }
            }
            _voucherVM.conditionTypes = Enum.GetValues(typeof(ConditionType)).Cast<ConditionType>().ToList();
            _voucherVM.valueTypes = Enum.GetValues(typeof(ValueType)).Cast<ValueType>().ToList();
            _voucherVM.expireTypes = Enum.GetValues(typeof(ExpireType)).Cast<ExpireType>().ToList();
        }

        public IActionResult Index()
        {
            ViewBag.Voucher = true;
            if (_voucherVM.vouchers != null)
                _voucherVM.vouchers = _voucherVM.vouchers.OrderBy(i => i.ValueType).ThenBy(i => i.CreatedDate).ToList();
            _voucherVM.filters = new List<string>();
            _voucherVM.filters.Add("");
            return View(_voucherVM);
        }

        [HttpPost]
        public IActionResult Sort(VoucherVM voucherVM)
        {
            List<Voucher> voucherFilters = new List<Voucher>();
            _voucherVM.filters = new List<string>();
            _voucherVM.filters.Add(voucherVM.categoryRadio);

            foreach (var i in _voucherVM.vouchers)
            {
                if (voucherVM.categoryRadio.Equals("All"))
                    voucherFilters.Add(i);
                else if (Enum.GetName(typeof(VoucherStatus), i.State).Equals(voucherVM.categoryRadio))
                    voucherFilters.Add(i);
            }

            voucherFilters = voucherFilters.OrderBy(i => i.ValueType).ToList();
            _voucherVM.page = _voucherVM.page - 1;
            ViewData["nProducts"] = _voucherVM.page;
            return PartialView("_ViewAll", voucherFilters);
        }

        [HttpGet]
        public IActionResult Create()
        {
            _voucherVM.voucher = new Voucher();

            return PartialView(_voucherVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(VoucherVM voucherVM)
        {
            var obj = voucherVM.voucher;

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
                    obj.ConditionValue = "0";
                }

                _voucherCRUD.CreateAsync(obj);

                _voucherVM.vouchers = _voucherCRUD.GetAllAsync().Result;
                foreach (var v in _voucherVM.vouchers)
                {
                    if (v.ExpiredValue == "0")
                    {
                        v.State = VoucherStatus.Expired;
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _voucherVM.vouchers) });
            }

            _voucherVM.voucher = obj;

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", _voucherVM) });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || id == "")
            {
                return NotFound();
            }
            var obj = await _voucherCRUD.GetByIdAsync(id);

            _voucherVM.voucher = obj;

            return PartialView(_voucherVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(VoucherVM voucherVM)
        {
            var obj = voucherVM.voucher;

            if (ModelState.IsValid)
            {
                if (obj.CreatedDate > DateTime.Now)
                {
                    obj.State = VoucherStatus.ComingSoon;
                }

                _voucherCRUD.Update(obj);

                _voucherVM.vouchers = _voucherCRUD.GetAllAsync().Result;

                foreach (var v in _voucherVM.vouchers)
                {
                    if (v.ExpiredValue == "0")
                    {
                        v.State = VoucherStatus.Expired;
                    }
                }

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _voucherVM.vouchers) });
            }

            _voucherVM.voucher = obj;

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", _voucherVM) });
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            Voucher? voucher = _voucherCRUD.GetByIdAsync(_voucherVM.voucher.Id).Result;

            _voucherVM.voucher = null;

            if (voucher != null)
            {
                _voucherCRUD.Remove(voucher);
            }
            _voucherVM.vouchers = _voucherCRUD.GetAllAsync().Result;
            foreach (var v in _voucherVM.vouchers)
            {
                if (v.ExpiredValue == "0")
                {
                    v.State = VoucherStatus.Expired;
                }
            }
            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _voucherVM.vouchers) });
        }
    }
}
