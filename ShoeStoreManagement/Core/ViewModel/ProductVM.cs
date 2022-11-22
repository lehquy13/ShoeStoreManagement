using ShoeStoreManagement.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Core.ViewModel
{
    public class ProductVM
    {
        public string ProductId { get; set; } = string.Empty;

        public Product Product { get; set; } = new Product();

        public int Amount { get; set; } = 1;

        public int Size { get; set; } = 0;

        public int AmountSelected { get; set; } = 1;

        public IFormFile Image { get; set; }


        [NotMapped]
        public List<string> TestSize { get; set; } = new List<string>();

        [NotMapped]
        public List<string> TestSizeAmount { get; set; } = new List<string>();

     
    }
}
