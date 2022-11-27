using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace ShoeStoreManagement.Core.ViewModel
{
    public class ProductVM
    {
        public string productId { get; set; } = string.Empty;

        public Product? product { get; set; }

        public List<Product>? products { get; set; }

        public List<ProductCategory>? productCategories { get; set; }

        public ApplicationUser? currentUser { get; set; }

        public int Amount { get; set; } = 1;

        public int Size { get; set; } = 0;

        public int AmountSelected { get; set; } = 1;

        public IFormFile? Image { get; set; } = null;

        [NotMapped]
        public List<string> TestSize { get; set; } = new List<string>();

        [NotMapped]
        public List<string> TestSizeAmount { get; set; } = new List<string>();

        [NotMapped]
        public Dictionary<int, int> SizeHashtable { get; set; } = new Dictionary<int, int>();   

        public List<SizeDetail>? sizes { get; set; }

        public List<string>? filters { get; set; }

        public int page { get; set; } = 1;

        public int nProducts { get; set; } = 0;
    }
}
