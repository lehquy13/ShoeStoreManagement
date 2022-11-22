using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Core.Models
{
    public class Image
    {
        [Key]
        public string ImageId { get; set; } = Guid.NewGuid().ToString();

        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }

        [DisplayName("Image Name")]
        [Column(TypeName = "nvarchar(100)")]
        public string ImageName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string ProductId { get; set; }

        [NotMapped]
        [DisplayName("Image File")]
        public IFormFile ImageFile { get; set; }
    }
}
