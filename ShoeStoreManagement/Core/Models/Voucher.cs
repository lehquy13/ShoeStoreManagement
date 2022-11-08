﻿using ShoeStoreManagement.Core.Enums;
using System.ComponentModel.DataAnnotations;
using ValueType = ShoeStoreManagement.Core.Enums.ValueType;

namespace ShoeStoreManagement.Core.Models
{
    public class Voucher
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public ExpireType ExpiredType { get; set; } = ExpireType.Amount;

        public string ExpiredValue { get; set; } = string.Empty;

        public ConditionType ConditionType { get; set; } = ConditionType.MinPrice;

        public string ConditionValue { get; set; } = string.Empty;

        public ValueType ValueType { get; set; } = ValueType.Percent;

        public int Value { get; set; } = 0;

        public VoucherStatus State { get; set; } = VoucherStatus.Using;
    }
}
