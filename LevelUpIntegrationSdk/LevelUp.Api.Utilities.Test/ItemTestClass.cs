using System.Collections.Generic;

namespace LevelUp.Api.Utilities.Test
{
    internal class ItemTestClass : IExemptableItem
    {
        public ItemTestClass(string sku, decimal totalInDollars, IEnumerable<IExemptableItem> children)
        {
            Sku = sku;
            TotalInDollars = totalInDollars;
            IsExempt = false;
            Children = children;
        }

        public IEnumerable<IExemptableItem> Children { get; set; }

        public bool IsExempt { get; set; }

        public string Sku { get; set; }

        public decimal? TotalInDollars { get; set; }

        public void AddChild(IExemptableItem childItem)
        {
            Children = new List<IExemptableItem>(Children)
                {
                    childItem,
                };
        }

        public void AddChildren(ICollection<IExemptableItem> children)
        {
            IList<IExemptableItem> temp = new List<IExemptableItem>(this.Children);
            foreach (IExemptableItem item in children)
            {
                temp.Add(item);
            }

            Children = temp;
        }
    }
}
