using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ShoeStoreManagement.Core.Models
{
    public class Address
    {
        string addressId = Guid.NewGuid().ToString();
        string addressDetail = "";
        string userId = "";
        string village = "";
        string district = "";
        string city = "";

        [Key]
        public string AddressId
        {
            get { return addressId; }
            set { }
        }

        [Required]
        public string AddressDetail
        {
            get { return addressDetail; }
            set { addressDetail = value; }
        }


        [Required]
        [ForeignKey("Id")]
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        [Required]
        public string Village
        {
            get { return village; }
            set { village = value; }
        }

        [Required]
        public string District
        {
            get { return district; }
            set { district = value; }
        }

        [Required]
        public string City
        {
            get { return city; }
            set { city = value; }
        }

    }
}
