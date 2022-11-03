using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ShoeStoreManagement.Core.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public byte[]? Bytes { get; set; }
        public string Description { get; set; } = string.Empty;
        public string FileExtension { get; set; } = string.Empty;
        public decimal Size { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
