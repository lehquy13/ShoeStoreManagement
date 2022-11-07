using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Core.Models
{
    public class SizeDetail
    {
        string sizeDetailId = Guid.NewGuid().ToString();
        string productId = string.Empty;
        int size = 0;
        int amount = 0;

        [Key]
        public string SizeDetailId
        {
            get { return sizeDetailId; }
            set { }
        }
        [Required]
        [ForeignKey("ProductId")]
        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        [NotMapped]
        public bool IsChecked { get; set; } = false;
    }
}
