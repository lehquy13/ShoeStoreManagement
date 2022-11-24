using ShoeStoreManagement.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace ShoeStoreManagement.Core.ViewModel
{
    public class ProductVM
    {
        public string ProductId { get; set; } = string.Empty;

        public Product Product { get; set; } = new Product();

        public int Amount { get; set; } = 1;

        public int Size { get; set; } = 0;

        public int AmountSelected { get; set; } = 1;

        public IFormFile Image { get; set; } = null;


        [NotMapped]
        public List<string> TestSize { get; set; } = new List<string>();

        [NotMapped]
        public List<string> TestSizeAmount { get; set; } = new List<string>();

        [NotMapped]
        public Dictionary<int, int> SizeHashtable { get; set; } = new Dictionary<int, int>();   
    }
}
