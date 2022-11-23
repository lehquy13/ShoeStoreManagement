using Microsoft.AspNetCore.Mvc;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.Core.ViewModel;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.CRUD.Interfaces;
using System.Security.Claims;

namespace ShoeStoreManagement.Controllers
{
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

            List<Image> img = _imageCRUD.GetAllByProductIdAsync(id).Result;

            if (img.Count > 0)
            {
                product.ImageName = img[0].ImageName;
            }

            _productVM.ProductId = product.ProductId;
            _productVM.Product = product;
            //_productVM.sizeDetails = sizeDetails;

            return View(_productVM);
        }

        [HttpPost]
        public void AddToCart(int amount, int size)
        {
            _productVM.AmountSelected = amount;
            _productVM.Size = size;

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart? cart = _cartCRUD.GetAsync(userId).Result;

            if (cart == null)
            {
                cart = new Cart();
                cart.UserId = userId;
                _cartCRUD.CreateAsync(cart);
            }

            Product? product = _productCRUD.GetByIdAsync(_productVM.ProductId).Result;

            if (product == null)
            {
                return;
            }

            CartDetail? cartDetail = _cartDetailCRUD.GetByProductIdAsync(_productVM.ProductId, cart.CartId, _productVM.Size).Result;

            if (cartDetail != null)
            {
                if (cartDetail.Amount < product.Amount)
                {
                    cartDetail.Amount += _productVM.AmountSelected;
                    cartDetail.CartDetailTotalSum = cartDetail.Amount * product.ProductUnitPrice;
                    _cartDetailCRUD.Update(cartDetail);
                }
            }
            else
            {
                cartDetail = new CartDetail()
                {
                    CartId = cart.CartId,
                    ProductId = _productVM.ProductId,
                    Amount = _productVM.AmountSelected,
                    Size = _productVM.Size,
                    CartDetailTotalSum = product.ProductUnitPrice * _productVM.AmountSelected,
                };

                _cartDetailCRUD.CreateAsync(cartDetail);
            }

            if (cartDetail.IsChecked)
            {
                cart.CartTotalAmountSelected += _productVM.AmountSelected;
                cart.CartTotalPrice = cartDetail.Amount * product.ProductUnitPrice;
            }

            cart.CartTotalAmount += _productVM.AmountSelected;


            return;
        }
    }
}
