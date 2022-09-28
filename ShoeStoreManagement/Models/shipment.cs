using MongoDB.Bson.Serialization.Attributes;

namespace ShoeStoreManagement.Models
{
    public class Shipment
    {
        Guid shipmentId = Guid.Empty;
        Guid orderID = Guid.Empty;
        DateTime shipmentDate = DateTime.Now;
        int shipmentPrice = 0;
        string shipmentState = String.Empty;

  
        public Guid ShipmentId
        {
            get { return shipmentId; }
            set { shipmentId = value; }
        }
        public Guid OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }
        public DateTime ShipmentDate
        {
            get { return shipmentDate; }
            set { shipmentDate = value; }
        }
        public int ShipmentPrice
        {
            get { return shipmentPrice; }
            set { shipmentPrice = value; }
        }
        public string ShipmentState
        {
            get { return shipmentState; }
            set { shipmentState = value; }
        }
    }
}
