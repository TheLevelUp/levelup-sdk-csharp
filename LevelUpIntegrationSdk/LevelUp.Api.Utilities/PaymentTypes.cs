using System;

namespace LevelUp.Api.Utilities
{
    [Flags]
    public enum PaymentTypes : int
    {
        None = 0,
        LevelUp = 1,
        Discount = 2,
        GiftCard = 4,
    }
}