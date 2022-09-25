using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class GoodCategory
    {
        string goodCategoryId = String.Empty;
        string goodCategoryName = String.Empty;

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string GoodCategoryId
        {
            get { return goodCategoryId; }
            set { goodCategoryId = value; }
        }
        public string GoodCategoryName
        {
            get { return goodCategoryName; }
            set { goodCategoryName = value; }
        }
    }
}
