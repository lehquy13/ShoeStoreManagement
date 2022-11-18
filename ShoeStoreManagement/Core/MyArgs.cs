namespace ShoeStoreManagement.Core
{
    public class MyArgs
    {
        public int myInt { get; set; } = 0;
        public int myOldInt { get; set; } = 0;

        public bool myBool { get; set; } = false;

        public float myFloat { get; set; } = 0;
        public float myOldFloat { get; set; } = 0;

        public bool isUpdateChecked { get; set; } = false;

        public bool isDeleted { get; set; } = false;

        public string myId { get; set; } = "";
    }
}
