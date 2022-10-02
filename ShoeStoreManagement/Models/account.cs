
using System.ComponentModel.DataAnnotations;

namespace ShoeStoreManagement.Models
{
    public class Account
    {
        string accountId = Guid.NewGuid().ToString();
        string accountUsername = String.Empty;
        string accountPassword = String.Empty;
        int accountType = 0;

        [Key]
        public string AccountId
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
