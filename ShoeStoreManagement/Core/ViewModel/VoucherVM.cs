using ShoeStoreManagement.Core.Enums;
using ShoeStoreManagement.Core.Models;
using ValueType = ShoeStoreManagement.Core.Enums.ValueType;

namespace ShoeStoreManagement.Core.ViewModel
{
	public class VoucherVM
	{
		public Voucher? voucher { get; set; }

		public List<Voucher>? vouchers { get; set; }

		public List<ConditionType>? conditionTypes { get; set; }

		public List<ValueType>? valueTypes { get; set; }

		public List<ExpireType>? expireTypes { get; set; }
	}
}
