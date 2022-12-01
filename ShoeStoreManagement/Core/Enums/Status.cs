namespace ShoeStoreManagement.Core.Enums;

public enum Status
{
    Waiting = 0,
    Delivering = 1,
    Delivered = 2,
    Canceled = 3,
    OnCharging = 4
}

public enum VoucherStatus
{
    Using = 0,
    Expired = -1,
    ComingSoon = 1,
}
