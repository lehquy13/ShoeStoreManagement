namespace ShoeStoreManagement.Core.Models
{
    public class Supplier
    {
        public string? _id { get; private set; }
        public string _name { get; set; } = string.Empty;
        public string _address { get; set; } = string.Empty;
        public string _products { get; set; } = string.Empty; // Type could change depends on how UI displays the suppliers
        public string _phoneNumber { get; set; } = string.Empty;
        public string _email { get; set; } = string.Empty;
        public string _note { get; set; } = string.Empty;
    }
}
