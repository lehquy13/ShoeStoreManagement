using ShoeStoreManagement.Core.Enums;
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
        [DisplayFormat(DataFormatString = "{0:DD-MM-YYYY}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Request Type Required")]
        public ExpireType ExpiredType { get; set; } = ExpireType.Amount;

        [Required]
        public string ExpiredValue { get; set; } = string.Empty;

        [Required]
        public ConditionType ConditionType { get; set; } = ConditionType.MinPrice;

        [Required]
        public string ConditionValue { get; set; } = string.Empty;

        [Required]
        public ValueType ValueType { get; set; } = ValueType.Percent;

        [Range(1, 200)]
        public int Value { get; set; } = 0;

        [Required]
        public VoucherStatus State { get; set; } = VoucherStatus.Using;
    }
}
