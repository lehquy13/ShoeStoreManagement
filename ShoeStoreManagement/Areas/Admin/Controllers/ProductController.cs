﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductCRUD _productCRUD;
        private readonly ISizeDetailCRUD _sizeDetailCRUD;
        private readonly IProductCategoryCRUD _productCategoryCRUD;
        private List<ProductCategory>? productCategories;
        private List<SizeDetail>? sizeDetails;
        private List<Product>? products;
        public ProductController(ILogger<ProductController> logger, IProductCRUD productCRUD
            ,IProductCategoryCRUD productCategoryCRUD, ISizeDetailCRUD sizeDetailCRUD)
        {
            _logger = logger;
            _productCRUD = productCRUD;
            _productCategoryCRUD = productCategoryCRUD;
            _sizeDetailCRUD = sizeDetailCRUD;
            Init();
        }

        private void Init()
        {
            productCategories = _productCategoryCRUD.GetAllAsync().Result;
            //sizeDetails = _productCRUD.SizeDetails.ToList<SizeDetail>();
            products = _productCRUD.GetAllAsync().Result;
            for (int i = 0; i < products.Count; i++)
            {
                //products[i].SetCategory(productCategories);
                //List<SizeDetail> sizeList = _sizeDetailCRUD.GetAllAsync(products[i].ProductId) as List<SizeDetail>;
                
            }
        }

        public IActionResult Index()
        {
            ViewBag.Product = true;
            ViewData["productCategories"] = productCategories;
            ViewData["products"] = products;
            return View();
        }

        public IActionResult Create()
        {
            //ViewBag.Product = true;
            return View();
        }

        public IActionResult Edit()
        {
            //ViewBag.Product = true;
            return View();
        }
    }
}
