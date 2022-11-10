using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Core.Models
{
    public class Supplier
    {
        string supplierid = Guid.NewGuid().ToString();
        string name = string.Empty;
        Address? address;
        string phoneNumber = string.Empty;
        string email = string.Empty;
        string note = string.Empty;

        [Key]
        public string SupplierId
        {
            get { return supplierid; }
            set { }
        }

        [Required]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [NotMapped]
        public Address? Address
        {
            get { return address; }
            set { address = value; }
        }

        [Phone]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        [EmailAddress]
        [Required]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }


    }
}
