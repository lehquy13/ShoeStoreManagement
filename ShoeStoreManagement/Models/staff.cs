using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Models
{
    public class Staff
    {
        string staffId = Guid.NewGuid().ToString();
        string accountId = String.Empty;
        string staffName = String.Empty;
        int staffAge = 0;
        int staffGender = 0;
        string staffAddress = String.Empty;
        string staffPhone = String.Empty;
        string staffEmail = String.Empty;
        string staffCitizenid = String.Empty;

        [Key]
        public string StaffId
        {
            get { return staffId; }
            set {  }
        }

        [Required]
        [ForeignKey("AccountId")]
        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }
        public string StaffName
        {
            get { return staffName; }
            set { staffName = value; }
        }
        public int StaffAge
        {
            get { return staffAge; }
            set { staffAge = value; }
        }
        public int StaffGender
        {
            get { return staffGender; }
            set { staffGender = value; }
        }
        public string StaffAddress
        {
            get { return staffAddress; }
            set { staffAddress = value; }
        }
        public string StaffPhone
        {
            get { return staffPhone; }
            set { staffPhone = value; }
        }
        public string StaffEmail
        {
            get { return staffEmail; }
            set { staffEmail = value; }
        }
        public string StaffCitizenid
        {
            get { return staffCitizenid; }
            set { staffCitizenid = value; }
        }
    }
}
