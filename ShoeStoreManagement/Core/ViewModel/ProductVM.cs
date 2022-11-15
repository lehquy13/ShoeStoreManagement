using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.Core.ViewModel
{
    public class ProductVM
    {
        public string ProductId { get; set; } = string.Empty;

        public Product Product { get; set; } = null;

        public int Amount { get; set; } = 1;

        public int Size { get; set; } = 0;

        public int AmountSelected { get; set; } = 1;
    }
}
