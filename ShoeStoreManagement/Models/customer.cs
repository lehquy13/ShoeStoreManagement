using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class Customer
    {
        string customerId = String.Empty;
        string accountId = String.Empty;
        string customerName = String.Empty;
        int customerAge = 0;
        string customerGender = String.Empty;
        string customerAddress = String.Empty;
        string customerPhone = String.Empty;
        string customerEmail = String.Empty;

        public string CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }
            
        public string AccountId
        {
            get { return customerId; }
            set { customerId = value; }
        }
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
