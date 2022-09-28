using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class CartDetail
    {
        Guid cartDetailId = Guid.Empty;
        Guid cartId = Guid.Empty;
        Guid productId = Guid.Empty;
        int cartDetailAmount = 0;
        int cartDetailTotalSum = 0;

        public Guid CartDetailId
        {
            get { return cartDetailId; }
            set { cartDetailId = value; }
        }
        public Guid CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        public Guid CartDetailProductId
        {
            get { return productId; }
            set { productId = value; }
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
