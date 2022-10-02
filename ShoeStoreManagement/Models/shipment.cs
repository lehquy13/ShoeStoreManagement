using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Models
{
    public class Shipment
    {
        string shipmentId = Guid.NewGuid().ToString();
        string orderId = String.Empty;
        DateTime shipmentDate = DateTime.Now;
        int shipmentPrice = 0;
        string shipmentState = String.Empty;

        [Key]
        public string ShipmentId
        {
            get { return shipmentId; }
            set { shipmentId = value; }
        }

        [Required]
        [ForeignKey("OrderId")]
        public string OrderId
        {
            get { return orderId; }
            set { orderId = value; }
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
