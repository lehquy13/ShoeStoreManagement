﻿
using ShoeStoreManagement.Core.Enums;
using ShoeStoreManagement.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStoreManagement.Core.Models;

public class Order
{

    string orderId = Guid.NewGuid().ToString();
    string userId = string.Empty;
    ApplicationUser? user = null;
    DateTime orderDate = DateTime.Now;
    List<OrderDetail> orderDetails = new List<OrderDetail>();

    float orderTotalPayment = 0;
    string orderVoucherId = string.Empty;
    Status status = Status.sampleStatus;
    DeliveryMethods deliveryMethod = DeliveryMethods.sampleMethod;
    long deliveryCharge = 0;

    string orderNote = string.Empty;
    //---------------------------------
    [NotMapped]
    public string DeliveryAddress { get; set; } = string.Empty;
    [Key]
    public string OrderId
    {
        get { return orderId; }
        set { }
    }

    [Required]
    [ForeignKey("Id")]
    public string UserId
    {
        get { return userId; }
        set { userId = value; }
    }

    public ApplicationUser? User
    {
        get { return user; }
        set { user = value; }
    }

    [NotMapped]
    public List<OrderDetail> OrderDetails
    {
        get { return orderDetails; }
        set { orderDetails = value; }
    }

    [DisplayFormat(DataFormatString = "{0:DD-MM-YYYY}")]
    public DateTime OrderDate
    {
        get { return orderDate; }
        set { orderDate = value; }
    }

    [DataType(DataType.Currency)]
    public float OrderTotalPayment
    {
        get { return orderTotalPayment; }
        set { orderTotalPayment = value; }
    }

    [Required]
    public Status Status
    {
        get { return status; }
        set { status = value; }
    }

    [Required]
    public DeliveryMethods DeliverryMethods
    {
        get { return deliveryMethod; }
        set { deliveryMethod = value; }
    }

    [Range(0, 99999)]
    [DataType(DataType.Currency)]
    public long DeliveryCharge
    {
        get { return deliveryCharge; }
        set { deliveryCharge = value; }
    }

    [ForeignKey("Id")]
    public string OrderVoucherId
    {
        get { return orderVoucherId; }
        set { orderVoucherId = value; }
    }

    public Voucher OrderVoucher { get; set; } = new Voucher();

    public string OrderNote
    {
        get { return orderNote; }
        set { orderNote = value; }
    }
}
