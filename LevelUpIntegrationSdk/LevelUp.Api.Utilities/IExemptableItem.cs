using System.Collections.Generic;

namespace LevelUp.Api.Utilities
{
    public interface IExemptableItem
    {
        IEnumerable<IExemptableItem> Children { get; }
        bool IsExempt { get; set; }
        string Sku { get; }
        decimal? TotalInDollars { get; }
    }
}
