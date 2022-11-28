
using ShoeStoreManagement.Core.Enums;
using ShoeStoreManagement.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.Core.ViewModel;

public class OrderVM
{
    public List<Product>? products { get; set; } = new List<Product>();

    public List<OrderDetail> currentOrderDetail { get; set; } = new List<OrderDetail>();

    public List<ApplicationUser>? customers { get; set; } = new List<ApplicationUser>();
    public ApplicationUser pickCustomers { get; set; } = new ApplicationUser();

    public Order? currOrder { get; set; } = new Order();

    public List<DeliveryMethods>? deliveryMethods { get; set; }

    public List<PaymentMethod>? paymentMethods { get; set; }

    public List<string> pickitems { get; set; } = new List<string>();

    public List<string> pickingQuantity { get; set; } = new List<string>();

    public List<string> pickingSize { get; set; } = new List<string>();

    public int totalAmount { get; set; } = 0;

    public float totalPayment { get; set; } = 0;

    public List<Order> allOrders { get; set; } = new List<Order>();

    public bool isOnProcessing { get; set; } = false;
}
