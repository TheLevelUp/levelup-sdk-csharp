using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace LevelUp.Api.Utilities.Test
{
    [TestClass]
    public class ExemptionsTest
    {
        private ICollection<IExemptableItem> _itemsList;

        public ExemptionsTest()
        {
            InitializeItemsList();
        }

        [TestInitialize]
        public void InitializeItemsList()
        {
            _itemsList = new Collection<IExemptableItem>()
                {
                    new ItemTestClass("1", 1.23m, null),
                    new ItemTestClass("A", 3.21m, new Collection<IExemptableItem>()
                        {
                            new ItemTestClass("99", 1.10m, null),
                            new ItemTestClass("76", 0.50m, null),
                            new ItemTestClass("77", 0.65m, null),
                            new ItemTestClass("67", 0.43m, new Collection<IExemptableItem>()
                                {
                                    new ItemTestClass("131", 0.02m, null),
                                    new ItemTestClass("313", 0.25m, null),
                                }),
                            new ItemTestClass("az", 4.56m, null),
                        }),
                    new ItemTestClass("2", 2.22m, null),
                };
        }

        [TestMethod]
        public void NullItem()
        {
            Exemptions.MarkExempted((IExemptableItem)null, new List<string> { "1", "99" }).Should().Be(0m);
        }

        [TestMethod]
        public void NullItemList()
        {
            Exemptions.MarkExempted((IEnumerable<IExemptableItem>)null, new List<string> {"1", "az"}).Should().Be(0m);
        }

        [TestMethod]
        public void NullItemInList()
        {
            List<IExemptableItem> listWithNull = new List<IExemptableItem>(_itemsList);
            listWithNull.Add((IExemptableItem)null);
            listWithNull.Add(new ItemTestClass("za", 3.41m, null));
            Exemptions.MarkExempted(listWithNull, new List<string> { "9", "atoz" }).Should().Be(0m);
        }

        [TestMethod]
        public void SingleTopLevelItem()
        {
            Exemptions.MarkExempted(_itemsList, new List<string> {"1"}).Should().Be(1.23m);
            _itemsList.First(i => i.Sku == "1").IsExempt.Should().BeTrue();
        }
        
        [TestMethod]
        public void MultipleItems()
        {
            Exemptions.MarkExempted(_itemsList, new List<string>{"az", "2", "313", "76"}).Should().Be(7.53m);
        }

        [TestMethod]
        public void MultipleTopLevelItems()
        {
            Exemptions.MarkExempted(_itemsList, new List<string> {"1", "2"}).Should().Be(3.45m);
            foreach (IExemptableItem item in _itemsList.Where(i => i.Sku == "1" || i.Sku == "2"))
            {
                item.IsExempt.Should().BeTrue();
            }
        }

        [TestMethod]
        public void SingleSubItem()
        {
            Exemptions.MarkExempted(_itemsList, new List<string> { "99" }).Should().Be(1.10m);
        }

        [TestMethod]
        public void MultipleSubItems()
        {
            Exemptions.MarkExempted(_itemsList, new List<string> { "az", "77", "67" }).Should().Be(5.64m);
        }

        [TestMethod]
        public void SingleSubSubItem()
        {
            Exemptions.MarkExempted(_itemsList, new List<string> {"131"}).Should().Be(0.02m);
        }

        [TestMethod]
        public void MultipleSubSubItems()
        {
            Exemptions.MarkExempted(_itemsList, new List<string> {"131", "313"}).Should().Be(0.27m);
        }
    }
}
