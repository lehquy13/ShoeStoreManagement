using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModel;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Drawing;
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
        private List<ProductCategory>? productCategories;
        private List<SizeDetail>? sizes;
        private IList<SelectListItem>? test;

        private List<Product>? products;
        private readonly UserManager<ApplicationUser> _usermanager;
        private ApplicationUser _currentUser;
        private Cart _userCart;
        //private static ProductVM _productVM = new ProductVM();
        private string categoryRadio = "";
        private string priceRadio = "";
        private int page = 1;



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
            productCategories = _productCategoryCRUD.GetAllAsync().Result;
            products = _productCRUD.GetAllAsync().Result;
            for (int i = 0; i < products.Count; i++)
            {
                products[i].SetCategory(productCategories);
                List<SizeDetail> sizeList = _sizeDetailCRUD.GetAllByIdAsync(products[i].ProductId).Result;
                int totalNumberShoeOfThatSize = 0;
                foreach (var obj in sizeList)
                {
                    obj.IsChecked = true;
                    //products[i].Sizes.Add(obj);
                    products[i].SizeHashtable.Add(obj.Size, obj.Amount);

                    totalNumberShoeOfThatSize += obj.Amount;
                }
                products[i].Amount = totalNumberShoeOfThatSize;
                List<Image> imgs = _imageCRUD.GetAllByProductIdAsync(products[i].ProductId).Result;
                if (imgs.Count > 0)
                {
                    products[i].ImageName = imgs[0].ImageName;
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

                SizeDetail sizeDetail = _sizeDetailCRUD.GetProductSizeAsync(product.ProductId, int.Parse(checkedSize[i])).Result;

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

        //[HttpPost]
        public IActionResult Index(string categoryRadio, string priceRadio, int page = 1)
        {
            List<Product> productFilter = new List<Product>();
            List<string> filters = new List<string>();
            filters.Add(categoryRadio);
            filters.Add(priceRadio);

            float minvalue = -1, maxvalue = -1;

            if (products.Count > 0)
            {
                if (!string.IsNullOrEmpty(priceRadio))
                {
                    string[] strsplt = priceRadio.Split('-', 2, StringSplitOptions.None);
                    minvalue = float.Parse(strsplt[0]);
                    maxvalue = float.Parse(strsplt[1]);
                }
            }

            foreach (Product i in products)
            {
                if ((i.ProductCategory.ProductCategoryName.Equals(categoryRadio)
                    || string.IsNullOrEmpty(categoryRadio)) && minCheck(minvalue, i.ProductUnitPrice)
                    && maxCheck(maxvalue, i.ProductUnitPrice))
                    productFilter.Add(i);
            }
            this.priceRadio = priceRadio;
            this.categoryRadio = categoryRadio;
            this.page = page;
            ViewBag.Product = true;
            ViewData["productCategories"] = productCategories;
            ViewData["products"] = productFilter;
            ViewData["filters"] = filters;
            ViewData["page"] = page;
            ViewData["test"] = test;
            return View(new Product());
        }

        [HttpPost]
        public IActionResult taolao(string categoryRadio,  string priceRadio, int page = 1)
        {
            List<Product> productFilter = new List<Product>();
            List<string> filters = new List<string>();
            filters.Add(categoryRadio);
            filters.Add(priceRadio);

            float minvalue = -1, maxvalue = -1;

            if (products.Count > 0)
            {
                if (!string.IsNullOrEmpty(priceRadio))
                {
                    string[] strsplt = priceRadio.Split('-', 2, StringSplitOptions.None);
                    minvalue = float.Parse(strsplt[0]);
                    maxvalue = float.Parse(strsplt[1]);
                }
            }

            foreach (Product i in products)
            {
                if ((i.ProductCategory.ProductCategoryName.Equals(categoryRadio)
                    || string.IsNullOrEmpty(categoryRadio)) && minCheck(minvalue, i.ProductUnitPrice)
                    && maxCheck(maxvalue, i.ProductUnitPrice))
                    productFilter.Add(i);
            }
            this.priceRadio = priceRadio;
            this.categoryRadio = categoryRadio;
            this.page = page;
            ViewBag.Product = true;
            ViewData["productCategories"] = productCategories;
            ViewData["products"] = productFilter;
            ViewData["filters"] = filters;
            ViewData["page"] = page;
            ViewData["test"] = test;
            return PartialView("ProductTable");
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            if (productVM.Product != null)
            {
                if (productVM.Product.ProductCategoryId != null)
                {
                    productVM.Product.ProductCategory = _productCategoryCRUD.GetByIdAsync(productVM.Product.ProductCategoryId).Result;//note
                }
                else
                {
                    return NotFound(productVM.Product.ProductCategoryId);

                }
                productVM.Product.TestSize = productVM.TestSize;
                productVM.Product.TestSizeAmount = productVM.TestSizeAmount;

                ModelState.Clear();

                if (TryValidateModel(productVM))
                {
                    var temp = productVM.Product.TestSizeAmount.Where(x => x != "0").ToList();

                    for (var i = 0; i < productVM.Product.TestSize.Count; i++)
                    {
                        _sizeDetailCRUD.CreateAsync(new SizeDetail()
                        {
                            Size = int.Parse(productVM.Product.TestSize[i]),
                            Amount = int.Parse(temp[i]),
                            ProductId = productVM.Product.ProductId
                        });
                    }

                    // Add image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(productVM.Image.FileName);
                    string extension = Path.GetExtension(productVM.Image.FileName);
                    fileName = fileName + extension;

                    Image image = new Image()
                    {
                        ImageName = fileName,
                        ImageFile = productVM.Image,
                        Title = "hi",
                        ProductId = productVM.Product.ProductId,
                    };

                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        image.ImageFile.CopyToAsync(fileStream);
                    }

                    _imageCRUD.CreateAsync(image);

                    productVM.Product.ImageName = fileName;

                    _productCRUD.CreateAsync(productVM.Product);

                    TempData["success"] = "Category is Created Successfully!!";
                    return RedirectToAction("Index");
                }
                return View(productVM);
            }

            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> Edit([Bind("ProductId,Product,TestSizeAmount,TestSize,Image")] ProductVM productVM)
        {
            if (productVM.Product.ProductCategoryId == null)
            {
                return NotFound(productVM.Product.ProductCategoryId);
            }
            else
                productVM.Product.ProductCategory = _productCategoryCRUD.GetByIdAsync(productVM.Product.ProductCategoryId).Result;//note

            productVM.Product.ImageName = "";

            if (productVM.Image == null)
            {
                using (var stream = new MemoryStream())
                {
                    productVM.Image = new FormFile(stream, 0, 0, "name", "fileName");
                }
            }

            ModelState.Clear();
            if (TryValidateModel(productVM))
            {
                productVM.Product.ProductId = productVM.ProductId;

                for (var i = 35; i <= 44; i++)
                {
                    var tempDetail = _sizeDetailCRUD.GetProductSizeAsync(productVM.ProductId, i).Result;
                    int amount = Int32.Parse(productVM.TestSizeAmount[i - 35]);
                    var newDetail = new SizeDetail() { Amount = amount, Size = i, ProductId = productVM.ProductId };

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
                                ProductId = productVM.Product.ProductId
                            });
                    }
                }
                productVM.Product.Sizes = await _sizeDetailCRUD.GetAllByIdAsync(productVM.Product.ProductId);
                productVM.Product.Amount = 0;
                foreach (var i in productVM.Product.Sizes)
                {
                    productVM.Product.Amount += i.Amount;
                }

                _productCRUD.Update(productVM.Product);

                if (productVM.Image.Length > 0)
                {
                    // Add image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(productVM.Image.FileName);
                    string extension = Path.GetExtension(productVM.Image.FileName);
                    fileName = fileName + extension;

                    Image image = new Image()
                    {
                        ImageName = fileName,
                        ImageFile = productVM.Image,
                        Title = "hi",
                        ProductId = productVM.ProductId,
                    };

                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await image.ImageFile.CopyToAsync(fileStream);
                    }

                    List<Image> imgs = _imageCRUD.GetAllByProductIdAsync(productVM.Product.ProductId).Result;

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
                            ProductId = productVM.ProductId,
                        });
                    }
                }

                await load();
                return PartialView("ProductTable");
            }
            return RedirectToAction("Index");
        }

        private async Task load()
        {
            List<Product> productFilter = new List<Product>();
            List<string> filters = new List<string>();
            filters.Add(categoryRadio);
            filters.Add(priceRadio);

            float minvalue = -1, maxvalue = -1;
            products = await _productCRUD.GetAllAsync();
            if (products.Count > 0)
            {
                if (!string.IsNullOrEmpty(priceRadio))
                {
                    string[] strsplt = priceRadio.Split('-', 2, StringSplitOptions.None);
                    minvalue = float.Parse(strsplt[0]);
                    maxvalue = float.Parse(strsplt[1]);
                }
            }

            foreach (Product i in products)
            {
                if ((i.ProductCategory.ProductCategoryName.Equals(categoryRadio) || string.IsNullOrEmpty(categoryRadio)) && minCheck(minvalue, i.ProductUnitPrice) && maxCheck(maxvalue, i.ProductUnitPrice))
                    productFilter.Add(i);
            }

            ViewBag.Product = true;
            ViewData["productCategories"] = productCategories;
            ViewData["products"] = productFilter;
            ViewData["filters"] = filters;
            ViewData["page"] = page;
            ViewData["test"] = test;

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
            return PartialView("ProductTable");
        }

        // Admin/Edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var obj = await _productCRUD.GetByIdAsync(id);
            if (obj != null)
            {
                obj.ProductCategory = _productCategoryCRUD.GetByIdAsync(obj.ProductCategoryId).Result;
                ViewData["productCategories"] = productCategories;

                ProductVM productVM = new ProductVM();
                productVM.Product = obj;
                productVM.ProductId = obj.ProductId;
                productVM.SizeHashtable = obj.SizeHashtable;

                return PartialView(productVM);

            }
            return RedirectToAction("Index");
        }
    }
}
