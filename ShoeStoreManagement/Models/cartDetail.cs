using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class CartDetail
    {
        string cartDetailId = String.Empty;
        string cartId = String.Empty;
        string goodId = String.Empty;
        int cartDetailAmount = 0;
        int cartDetailTotalSum = 0;

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string CartDetailId
        {
            get { return cartDetailId; }
            set { cartDetailId = value; }
        }
        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        public string CartDetailGoodsId
        {
            get { return goodId; }
            set { goodId = value; }
        }
        public int CartDetailAmount
        {
            get { return cartDetailAmount; }
            set { cartDetailAmount = value; }
        }
        public int CartDetailTotalSum
        {
            get { return cartDetailTotalSum; }
            set { cartDetailTotalSum = value; }
        }
    }
}
