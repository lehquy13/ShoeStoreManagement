using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModel;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Data;
using System.Security.Claims;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DetailProductController : Controller
    {
        private readonly IProductCRUD _productCRUD;
        private readonly ICartCRUD _cartCRUD;
        private readonly ICartDetailCRUD _cartDetailCRUD;
        private readonly ISizeDetailCRUD _sizeDetailCRUD;
        private readonly IImageCRUD _imageCRUD;
        private static ProductVM _productVM = new ProductVM();

        public DetailProductController(IProductCRUD productCRUD, ISizeDetailCRUD sizeDetailCRUD, ICartCRUD cartCRUD, ICartDetailCRUD cartDetailCRUD, IImageCRUD imageCRUD)
        {
            _productCRUD = productCRUD;
            _sizeDetailCRUD = sizeDetailCRUD;
            _cartCRUD = cartCRUD;
            _cartDetailCRUD = cartDetailCRUD;
            _imageCRUD = imageCRUD;
        }

        [HttpGet]
        public IActionResult Index(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? product = _productCRUD.GetByIdAsync(id).Result;

            if (product == null)
            {
                return NotFound();
            }

            List<SizeDetail> sizeDetails = _sizeDetailCRUD.GetAllByIdAsync(product.ProductId).Result;

            product.Sizes = sizeDetails;

            _productVM.productId = product.ProductId;
            _productVM.product = product;
            
            return View(_productVM);
        }

        [HttpPost]
        public IActionResult LoadAmountOfSize(int size)
        {
            _productVM.Size = size;

            SizeDetail? sizeDetail = _sizeDetailCRUD.GetProductSizeAsync(_productVM.productId, size).Result;

            if(sizeDetail == null)
            {
                return NotFound();
            }

            _productVM.Amount = sizeDetail.Amount;

            return Redirect("Index/" + _productVM.productId);
        }

        [HttpPost]
        public IActionResult CreateCartItem(ProductVM productVM)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart? cart = _cartCRUD.GetAsync(userId).Result;

            if (cart == null)
            {
                cart = new Cart();
                cart.UserId = userId;
                _cartCRUD.CreateAsync(cart);
            }

            Product? product = _productCRUD.GetByIdAsync(_productVM.productId).Result;

            if (product == null)
            {
                return NotFound();
            }

            CartDetail? cartDetail = _cartDetailCRUD.GetByProductIdAsync(_productVM.productId, cart.CartId, productVM.Size).Result;

            if (cartDetail != null)
            {
                if (cartDetail.Amount < product.Amount)
                {
                    cartDetail.Amount += productVM.AmountSelected;
                    cartDetail.CartDetailTotalSum += cartDetail.Amount * product.ProductUnitPrice;
                    _cartDetailCRUD.Update(cartDetail);
                }
            }
            else
            {
                cartDetail = new CartDetail()
                {
                    CartId = cart.CartId,
                    ProductId = _productVM.productId,
                    Amount = productVM.AmountSelected,
                    Size = productVM.Size,
                    CartDetailTotalSum = product.ProductUnitPrice,
                };

                _cartDetailCRUD.CreateAsync(cartDetail);
            }

            return Redirect("Index/" + _productVM.productId);
        }
    }
}
