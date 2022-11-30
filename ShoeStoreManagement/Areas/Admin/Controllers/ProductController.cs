using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModel;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Security.Claims;
using Image = ShoeStoreManagement.Core.Models.Image;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private const int minusIndex = 35;

        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductCRUD _productCRUD;
        private readonly ICartCRUD _cartCRUD;
        private readonly ICartDetailCRUD _cartDetailCRUD;
        private readonly ISizeDetailCRUD _sizeDetailCRUD;
        private readonly IProductCategoryCRUD _productCategoryCRUD;
        private readonly IImageCRUD _imageCRUD;
        //private IList<SelectListItem>? test;

        private readonly UserManager<ApplicationUser> _usermanager;
        private static ProductVM _productVM = new ProductVM();



        public ProductController(ILogger<ProductController> logger, IProductCRUD productCRUD, UserManager<ApplicationUser> usermanager
            , IProductCategoryCRUD productCategoryCRUD, ISizeDetailCRUD sizeDetailCRUD, ICartCRUD cartCRUD, ICartDetailCRUD cartDetailCRUD, IWebHostEnvironment webHostEnvironment, IImageCRUD imageCRUD)
        {
            _logger = logger;
            _productCRUD = productCRUD;
            _productCategoryCRUD = productCategoryCRUD;
            _sizeDetailCRUD = sizeDetailCRUD;
            _cartCRUD = cartCRUD;
            _cartDetailCRUD = cartDetailCRUD;
            _imageCRUD = imageCRUD;
            _usermanager = usermanager;
            _hostEnvironment = webHostEnvironment;
            Init();
        }

        private void Init()
        {
            _productVM.productCategories = _productCategoryCRUD.GetAllAsync().Result;
            _productVM.products = _productCRUD.GetAllAsync().Result;
            for (int i = 0; i < _productVM.products.Count; i++)
            {
                _productVM.products[i].SetCategory(_productVM.productCategories);
                List<SizeDetail> sizeList = _sizeDetailCRUD.GetAllByIdAsync(_productVM.products[i].ProductId).Result;
                _productVM.products[i].Sizes = sizeList;
               // int totalNumberShoeOfThatSize = 0;
                //for (int j = 0; j < sizeList.Count; j++)
                //{
                //    sizeList[j].IsChecked = true;
                //    //products[i].Sizes.Add(obj);
                //    //_productVM.products[i].SizeHashtable.Add(sizeList[j].Size, sizeList[j].Amount);

                //    totalNumberShoeOfThatSize += sizeList[j].Amount;
                //}
                //_productVM.products[i].Amount = totalNumberShoeOfThatSize;
                List<Image> imgs = _imageCRUD.GetAllByProductIdAsync(_productVM.products[i].ProductId).Result;

                if (imgs.Count > 0)
                {
                    _productVM.products[i].ImageName = imgs[0].ImageName;
                }
            }

        }

        [HttpPost]
        public IActionResult ToCart(Product product)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart? cart = _cartCRUD.GetAsync(userId).Result;

            if (cart == null)
            {
                cart = new Cart();
                cart.UserId = userId;
                _cartCRUD.CreateAsync(cart);
            }

            //Product product = _productCRUD.GetByIdAsync(productID).Result; 

            if (product == null)
            {
                return NotFound();
            }

            List<SizeDetail> sizes = _sizeDetailCRUD.GetAllByIdAsync(product.ProductId).Result;

            List<string> checkedSize = product.TestSize; // Store checked sizes
            List<string> checkedSizeAmount = new List<string>(); // Store amount of checked sizes

            foreach (string i in product.TestSizeAmount) // Remove null indexes
            {
                if (!string.IsNullOrEmpty(i))
                    checkedSizeAmount.Add(i);
            }

            // Handle changes in size's amount
            for (int i = 0; i < checkedSize.Count; i++)
            {
                if (checkedSizeAmount[i] == null)
                    continue;

                SizeDetail? sizeDetail = _sizeDetailCRUD.GetProductSizeAsync(product.ProductId, int.Parse(checkedSize[i])).Result;

                if (sizeDetail == null) return NotFound();

                sizeDetail.Amount -= int.Parse(checkedSizeAmount[i]);
                _sizeDetailCRUD.Update(sizeDetail);

                CartDetail? cartDetail = null;

                foreach (CartDetail cd in _cartDetailCRUD.GetAllAsync(cart.CartId).Result) // Find Cart Detail of Chosen Product with the Size we need
                {
                    if (cd.ProductId.Equals(product.ProductId) && cd.Size == sizeDetail.Size)
                        cartDetail = cd;
                }

                if (cartDetail != null)
                {
                    cartDetail.Amount += int.Parse(checkedSizeAmount[i]);
                    cartDetail.CartDetailTotalSum = cartDetail.Amount * product.ProductUnitPrice;
                    _cartDetailCRUD.Update(cartDetail);
                }
                else
                {
                    CartDetail newCartDetail = new CartDetail()
                    {
                        CartId = cart.CartId,
                        ProductId = product.ProductId,
                        Amount = int.Parse(checkedSizeAmount[i]),
                        CartDetailTotalSum = int.Parse(checkedSizeAmount[i]) * product.ProductUnitPrice,
                        Size = int.Parse(checkedSize[i]),
                    };

                    _cartDetailCRUD.CreateAsync(newCartDetail);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ChooseSize(string id) //This is Product's ID 
        {
            Product? product = _productCRUD.GetByIdAsync(id).Result;

            if (product == null)
            {
                return NotFound();
            }

            List<SizeDetail> sizes = _sizeDetailCRUD.GetAllByIdAsync(id).Result;

            ViewData["chosenProductSizes"] = sizes;

            return PartialView(product);
        }

        [HttpGet]
        public IActionResult Index(string categoryRadio, string priceRadio , int page = 1)
        {
            _productVM.products = _productVM.products.OrderBy(o => o.ProductName).ToList();
            List<Product> productFilter = new List<Product>();
            _productVM.filters = new List<string>();
            _productVM.filters.Add(categoryRadio);
            _productVM.filters.Add(priceRadio);

            float minvalue = -1, maxvalue = -1;

            if (_productVM.products.Count > 0)
            {
                if (!string.IsNullOrEmpty(priceRadio) && !priceRadio.Equals("All"))
                {
                    string[] strsplt = priceRadio.Split('-', 2, StringSplitOptions.None);
                    minvalue = float.Parse(strsplt[0]);
                    maxvalue = float.Parse(strsplt[1]);
                }
            }

            foreach (var i in _productVM.products)
            {
                if ((i.ProductCategory.ProductCategoryName.Equals(categoryRadio)
                    || string.IsNullOrEmpty(categoryRadio)) && minCheck(minvalue, i.ProductUnitPrice)
                    && maxCheck(maxvalue, i.ProductUnitPrice))
                    productFilter.Add(i);
            }
            
            _productVM.products= productFilter;
            ViewBag.Product = true;
            _productVM.page = page-1;
            ViewData["nProducts"] = _productVM.page;

			return View(_productVM);
        }

        [HttpPost]
        public IActionResult Pagination(string page = "1")
        {
       
            _productVM.page = Int32.Parse(page )- 1;
            ViewData["nProducts"] = _productVM.page;
            return PartialView("_Pagination", _productVM);
        }

        [HttpPost]
        public IActionResult Sort(ProductVM productVM)
        {
            List<Product> productFilter = new List<Product>();
            _productVM.filters = new List<string>();
            _productVM.filters.Add(productVM.categoryRadio);
            _productVM.filters.Add(productVM.priceRadio);

            float minvalue = -1, maxvalue = -1;

            if (_productVM.products.Count > 0)
            {
                if (!string.IsNullOrEmpty(productVM.priceRadio) && !productVM.priceRadio.Equals("All"))
                {
                    string[] strsplt = productVM.priceRadio.Split('-', 2, StringSplitOptions.None);
                    minvalue = float.Parse(strsplt[0]);
                    maxvalue = float.Parse(strsplt[1]);
                }
            }

            foreach (var i in _productVM.products)
            {
                if ((i.ProductCategory.ProductCategoryName.Equals(productVM.categoryRadio)
                    || string.IsNullOrEmpty(productVM.categoryRadio)) && minCheck(minvalue, i.ProductUnitPrice)
                    && maxCheck(maxvalue, i.ProductUnitPrice))
                    productFilter.Add(i);
            }

            productFilter = productFilter.OrderBy(i => i.ProductName).ToList();
            _productVM.page = productVM.page -1;
			ViewData["nProducts"] = _productVM.page;
			return PartialView("_ViewAll", productFilter);
        }



        private bool minCheck(float value, float price)
        {
            if (value == -1)
                return true;

            if (price >= value)
                return true;
            else
                return false;
        }

        private bool maxCheck(float value, float price)
        {
            if (value == -1)
                return true;

            if (price < value)
                return true;
            else
                return false;
        }

        [HttpGet]
        public IActionResult Create()
        {
            _productVM.product = new Product();

            return PartialView(_productVM);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            var product = productVM.product;

            if (product != null)
            {
                if (product.ProductCategoryId != null)
                {
                    product.ProductCategory = _productCategoryCRUD.GetByIdAsync(product.ProductCategoryId).Result ?? new ProductCategory();//note
                }
                else
                {
                    return NotFound(product.ProductCategoryId);

                }

                if (product.Image == null)
                {
                    using (var stream = new MemoryStream())
                    {
                        product.Image = new FormFile(stream, 0, 0, "name", "fileName");
                    }
                }

                ModelState.Clear();
                if (TryValidateModel(product))
                {
                    var temp = productVM.TestSizeAmount.Where(x => x != "0").ToList();

                    for (var i = 0; i < productVM.TestSize.Count; i++)
                    {
                        _sizeDetailCRUD.CreateAsync(new SizeDetail()
                        {
                            Size = int.Parse(productVM.TestSize[i]),
                            Amount = int.Parse(temp[i]),
                            ProductId = product.ProductId
                        });

                        product.Amount += int.Parse(temp[i]);
                    }

                    string fileName = "";

                    // Add image
                    if (product.Image.Length > 0)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        fileName = Path.GetFileNameWithoutExtension(product.Image.FileName);
                        string extension = Path.GetExtension(product.Image.FileName);
                        fileName = fileName + extension;

                        Image image = new Image()
                        {
                            ImageName = fileName,
                            ImageFile = product.Image,
                            Title = "hi",
                            ProductId = product.ProductId,
                        };

                        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            image.ImageFile.CopyToAsync(fileStream);
                        }

                        _imageCRUD.CreateAsync(image);
                    }

                    product.ImageName = fileName;

                    _productCRUD.CreateAsync(product);

                    _productVM.products = _productCRUD.GetAllAsync().Result.OrderBy(o => o.ProductName).ToList();
                    //_productVM.page = 0;

					//_productVM.nProducts = _productVM.page;
					ViewData["nProducts"] = _productVM.page;

					TempData["success"] = "Category is Created Successfully!!";

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _productVM.products) });
                }
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", _productVM) });
            }
            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> Edit([Bind("ProductId,product,TestSizeAmount,TestSize,Image")] ProductVM productVM)
        {
            var product = productVM.product;

            if (product.ProductCategoryId == null)
            {
                return NotFound(product.ProductCategoryId);
            }
            else
            {
                product.ProductCategory = _productCategoryCRUD.GetByIdAsync(product.ProductCategoryId).Result ?? new ProductCategory();//note

            }

            if (product.Image == null)
            {
                using (var stream = new MemoryStream())
                {
                    product.Image = new FormFile(stream, 0, 0, "name", "fileName");
                }
            }

            ModelState.Clear();
            if (TryValidateModel(product))
            {

                for (var i = 35; i <= 44; i++)
                {
                    var tempDetail = _sizeDetailCRUD.GetProductSizeAsync(product.ProductId, i).Result;
                    int amount = Int32.Parse(productVM.TestSizeAmount[i - 35]);
                    var newDetail = new SizeDetail() { Amount = amount, Size = i, ProductId = product.ProductId };

                    if (tempDetail != null)
                    {
                        if (amount > 0 && amount != tempDetail.Amount)
                        {
                            await _sizeDetailCRUD.Update(newDetail);

                        }
                        else if (amount == 0 || !productVM.TestSize.Contains(i.ToString()))
                        {
                            _sizeDetailCRUD.Remove(newDetail);
                        }
                    }
                    else
                    {
                        if (amount > 0)
                            await _sizeDetailCRUD.CreateAsync(new SizeDetail()
                            {
                                Size = i,
                                Amount = amount,
                                ProductId = product.ProductId
                            });
                    }
                }
                productVM.product.Sizes = await _sizeDetailCRUD.GetAllByIdAsync(productVM.product.ProductId);
                productVM.product.Amount = 0;
                foreach (var i in productVM.product.Sizes)
                {
                    productVM.product.Amount += i.Amount;
                }

                _productCRUD.Update(product);

                if (product.Image.Length > 0)
                {
                    // Add image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(product.Image.FileName);
                    string extension = Path.GetExtension(product.Image.FileName);
                    fileName = fileName + extension;

                    Image image = new Image()
                    {
                        ImageName = fileName,
                        ImageFile = product.Image,
                        Title = "hi",
                        ProductId = product.ProductId,
                    };

                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await image.ImageFile.CopyToAsync(fileStream);
                    }

                    List<Image> imgs = _imageCRUD.GetAllByProductIdAsync(product.ProductId).Result;

                    if (imgs.Count > 0)
                    {
                        imgs[0].ImageName = image.ImageName;
                        imgs[0].Title = "updated";
                        _imageCRUD.Update(imgs[0]);
                    }
                    else
                    {
                        await _imageCRUD.CreateAsync(new Image()
                        {
                            ImageName = image.ImageName,
                            Title = "new",
                            ProductId = product.ProductId,
                        });
                    }
                }

                await load();
                return PartialView("_ViewAll", _productVM.products);
            }
            return RedirectToAction("Index");
        }

        private async Task load()
        {
            List<Product> productFilter = new List<Product>();
            List<string> filters = new List<string>();
            filters.Add(_productVM.categoryRadio);
            filters.Add(_productVM.priceRadio);

            float minvalue = -1, maxvalue = -1;
            _productVM.products = await _productCRUD.GetAllAsync();
            if (_productVM.products.Count > 0)
            {
                if (!string.IsNullOrEmpty(_productVM.priceRadio))
                {
                    string[] strsplt = _productVM.priceRadio.Split('-', 2, StringSplitOptions.None);
                    minvalue = float.Parse(strsplt[0]);
                    maxvalue = float.Parse(strsplt[1]);
                }
            }

            foreach (Product i in _productVM.products)
            {
                if ((i.ProductCategory.ProductCategoryName.Equals(_productVM.categoryRadio) || string.IsNullOrEmpty(_productVM.categoryRadio)) && minCheck(minvalue, i.ProductUnitPrice) && maxCheck(maxvalue, i.ProductUnitPrice))
                    productFilter.Add(i);
            }
            _productVM.products = productFilter;
            _productVM.products = _productVM.products.OrderBy(o=>o.ProductName).ToList();
            _productVM.page = 0;//reseet nhá
            ViewBag.Product = true;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var obj = await _productCRUD.GetByIdAsync(id);
            if (obj != null)
            {
                _sizeDetailCRUD.DeleteAllDetailsByIdAsync(obj.ProductId);
                _productCRUD.Remove(obj);
            }

            await load();
            return PartialView("_ViewAll", _productVM.products);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var obj = await _productCRUD.GetByIdAsync(id);
            if (obj != null)
            {
                obj.ProductCategory = _productCategoryCRUD.GetByIdAsync(obj.ProductCategoryId).Result ?? new ProductCategory();


                List<SizeDetail> sizeList = _sizeDetailCRUD.GetAllByIdAsync(obj.ProductId).Result;
                int totalNumberShoeOfThatSize = 0;
                for (int j = 0; j < sizeList.Count; j++)
                {
                    sizeList[j].IsChecked = true;
                    //products[i].Sizes.Add(obj);
                    obj.SizeHashtable.Add(sizeList[j].Size, sizeList[j].Amount);

                    totalNumberShoeOfThatSize += sizeList[j].Amount;
                }
                obj.Amount = totalNumberShoeOfThatSize;
                List<Image> imgs = _imageCRUD.GetAllByProductIdAsync(obj.ProductId).Result;

                if (imgs.Count > 0)
                {
                    obj.ImageName = imgs[0].ImageName;
                }

                _productVM.product = obj;

                return PartialView(_productVM);

            }
            return RedirectToAction("Index");
        }


    }
}
