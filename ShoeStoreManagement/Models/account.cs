using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class Account
    {
        Guid accountId = Guid.Empty;
        string accountUsername = String.Empty;
        string accountPassword = String.Empty;
        int accountType = 0;

        public Guid AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }
        public string AccountUsername
        {
            get { return accountUsername; }
            set { accountUsername = value; }
        }
        public string AccountPassword
        {
            get { return accountPassword; }
            set { accountPassword = value; }
        }
        public int AccountType
        {
            get { return accountType; }
            set { accountType = value; }
        }
    }
}
