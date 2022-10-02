
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Models
{
    public class Customer
    {
        string customerId = Guid.NewGuid().ToString();

        string accountId = String.Empty;
        // account ojbect wont be add to db somehow
        Account account = new Account();

        // cart ojbect wont be add to db somehow
        Cart cart = new Cart();

        string customerName = String.Empty;
        int customerAge = 0;
        string customerGender = String.Empty;
        string customerAddress = String.Empty;
        string customerPhone = String.Empty;
        string customerEmail = String.Empty;

        [Key]
        public string CustomerId
        {
            get { return customerId.ToString(); }
            set { customerId = value; }
        }

        [Required]
        [ForeignKey("AccountId")]
        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        public Account Account
        {
            get { return account; }
            set { account = value; }
        }

        public Cart Cart
        {
            get { return cart; }
            set { cart = value; }
        }

        [DisplayFormat(NullDisplayText = "Username")]
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        public int CustomerAge
        {
            get { return customerAge; }
            set { customerAge = value; }
        }

        public string CustomerGender
        {
            get { return customerGender; }
            set { customerGender = value; }
        }


        public string CustomerAddress
        {
            get { return customerAddress; }
            set { customerAddress = value; }
        }


        public string CustomerPhone
        {
            get { return customerPhone; }
            set { customerPhone = value; }
        }

        public string CustomerEmail
        {
            get { return customerEmail; }
            set { customerEmail = value; }
        }
    }
}
