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

        public string Supplierid
        {
            get { return supplierid; }
            set { }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Address? Address
        {
            get { return address; }
            set { address = value; }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
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
