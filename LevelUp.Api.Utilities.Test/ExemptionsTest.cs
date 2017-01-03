#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ExemptionsTest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2016 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
// </copyright>
// <license publisher="Apache Software Foundation" date="January 2004" version="2.0">
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
//   in compliance with the License. You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software distributed under the License
//   is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//   or implied. See the License for the specific language governing permissions and limitations under
//   the License.
// </license>
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Utilities.Test
{
    [TestClass]
    public class ExemptionsTest
    {
        private static IEnumerable<IExemptableItem> FlattenCollection(IEnumerable<IExemptableItem> collectionToFlatten)
        {
            if (null == collectionToFlatten)
            {
                return null;
            }

            IList<IExemptableItem> flattened = new List<IExemptableItem>();

            Stack<IExemptableItem> stack = new Stack<IExemptableItem>(collectionToFlatten);

            while (stack.Count > 0)
            {
                IExemptableItem item = stack.Pop();

                if (null == item)
                {
                    continue;
                }

                flattened.Add(new ItemTestClass(item.Sku, item.ChargedPriceTotalInDollars, item.IsExempt, null));

                if (null != item.Children)
                {
                    foreach (IExemptableItem child in item.Children)
                    {
                        stack.Push(child);
                    }
                }
            }

            return flattened;
        }

        [TestMethod]
        public void TestFlattenMethod()
        {
            IEnumerable<IExemptableItem> deepCollection = new Collection<IExemptableItem>()
                    {
                        new ItemTestClass("1", 1.23m),
                        new ItemTestClass("A", 3.21m, false, new Collection<IExemptableItem>()
                            {
                                new ItemTestClass("99", 1.10m),
                                new ItemTestClass("76", 0.50m),
                                new ItemTestClass("77", 0.65m),
                                new ItemTestClass("67", 0.43m, false, new Collection<IExemptableItem>()
                                    {
                                        new ItemTestClass("131", 0.02m),
                                        new ItemTestClass("313", 0.25m),
                                    }),
                                new ItemTestClass("az", 4.56m),
                            }),
                        new ItemTestClass("2", 2.22m),
                    };

            IEnumerable<IExemptableItem> flatttenedCollection = new Collection<IExemptableItem>()
                {
                    new ItemTestClass("1", 1.23m),
                    new ItemTestClass("A", 3.21m),
                    new ItemTestClass("99", 1.10m),
                    new ItemTestClass("76", 0.50m),
                    new ItemTestClass("77", 0.65m),
                    new ItemTestClass("67", 0.43m),
                    new ItemTestClass("131", 0.02m),
                    new ItemTestClass("313", 0.25m),
                    new ItemTestClass("az", 4.56m),
                    new ItemTestClass("2", 2.22m),
                };

            var flat = FlattenCollection(deepCollection);

            flat.Should().NotBeNull();
            flat.Should().NotBeEmpty();
            flat.Count().Should().Be(flatttenedCollection.Count());
            flat.ShouldAllBeEquivalentTo(flatttenedCollection);
        }
            
        [TestClass]
        public class MarkExemptedTests
        {
            private ICollection<IExemptableItem> _itemsList;

            public MarkExemptedTests()
            {
                InitializeItemsList();
            }

            [TestInitialize]
            public void InitializeItemsList()
            {
                _itemsList = new Collection<IExemptableItem>()
                    {
                        new ItemTestClass("1", 1.23m),
                        new ItemTestClass("A", 3.21m, false, new Collection<IExemptableItem>()
                            {
                                new ItemTestClass("99", 1.10m),
                                new ItemTestClass("76", 0.50m),
                                new ItemTestClass("77", 0.65m),
                                new ItemTestClass("67", 0.43m, false, new Collection<IExemptableItem>()
                                    {
                                        new ItemTestClass("131", 0.02m),
                                        new ItemTestClass("313", 0.25m),
                                    }),
                                new ItemTestClass("az", 4.56m),
                            }),
                        new ItemTestClass("2", 2.22m),
                    };
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void NullItem()
            {
                IEnumerable<IExemptableItem> items = new Collection<IExemptableItem>
                    {
                        (IExemptableItem)null,
                    };
                
                Exemptions.MarkExempted(items, new List<string> {"1", "99"});

                FlattenCollection(items).Where(i => i!= null && i.IsExempt).Should().BeEmpty();
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void NullItemList()
            {
                IEnumerable<IExemptableItem> items = null;

                //Should not throw
                Exemptions.MarkExempted(items, new List<string> {"1", "az"});
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void NullItemInList()
            {
                List<IExemptableItem> listWithNull = new List<IExemptableItem>(_itemsList);
                listWithNull.Add((IExemptableItem)null);
                listWithNull.Add(new ItemTestClass("za", 3.41m));

                Exemptions.MarkExempted(listWithNull, new List<string> { "9", "atoz" });

                FlattenCollection(listWithNull).Where(i => i != null && i.IsExempt).Should().BeEmpty();
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void EmptyItemList()
            {
                List<IExemptableItem> empty = new List<IExemptableItem>();

                Exemptions.MarkExempted(empty, new List<string> { "9", "atoz" });

                FlattenCollection(empty).Where(i => i != null && i.IsExempt).Should().BeEmpty();
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void NullExemptList()
            {
                Exemptions.MarkExempted(_itemsList, null);

                FlattenCollection(_itemsList).Count(i => i.IsExempt).Should().Be(0);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void EmptyExemptList()
            {
                Exemptions.MarkExempted(_itemsList, new List<string>());

                FlattenCollection(_itemsList).Count(i => i.IsExempt).Should().Be(0);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void SingleTopLevelItem()
            {
                var exemptedItemsList = new List<string> { "1" };
                const int expectedExemptItemCount = 1;

                FlattenCollection(_itemsList).Count(i => i.IsExempt).Should().Be(0);

                Exemptions.MarkExempted(_itemsList, exemptedItemsList);

                var flat = FlattenCollection(_itemsList);
                flat.Count(i => i.IsExempt).Should().Be(expectedExemptItemCount);
                flat.First(i => i.Sku == "1").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "A").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "2").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "99").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "76").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "77").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "67").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "131").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "313").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "az").IsExempt.Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void MultipleItems()
            {
                var exemptedItemsList = new List<string> { "az", "2", "313", "76" };
                const int expectedExemptItemCount = 4;

                FlattenCollection(_itemsList).Count(i => i.IsExempt).Should().Be(0);

                Exemptions.MarkExempted(_itemsList, exemptedItemsList);

                var flat = FlattenCollection(_itemsList);
                flat.Count(i => i.IsExempt).Should().Be(expectedExemptItemCount);
                flat.First(i => i.Sku == "1").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "A").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "2").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "99").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "76").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "77").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "67").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "131").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "313").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "az").IsExempt.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void MultipleTopLevelItems()
            {
                var exemptedItemsList = new List<string> { "1", "2" };
                const int expectedExemptItemCount = 2;

                FlattenCollection(_itemsList).Count(i => i.IsExempt).Should().Be(0);

                Exemptions.MarkExempted(_itemsList, exemptedItemsList);

                var flat = FlattenCollection(_itemsList);
                flat.Count(i => i.IsExempt).Should().Be(expectedExemptItemCount);
                flat.First(i => i.Sku == "1").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "A").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "2").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "99").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "76").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "77").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "67").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "131").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "313").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "az").IsExempt.Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void SingleSubItem()
            {
                var exemptedItemsList = new List<string> { "99" };
                const int expectedExemptItemCount = 1;

                FlattenCollection(_itemsList).Count(i => i.IsExempt).Should().Be(0);

                Exemptions.MarkExempted(_itemsList, exemptedItemsList);

                var flat = FlattenCollection(_itemsList);
                flat.Count(i => i.IsExempt).Should().Be(expectedExemptItemCount);
                flat.First(i => i.Sku == "1").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "A").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "2").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "99").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "76").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "77").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "67").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "131").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "313").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "az").IsExempt.Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void MultipleSubItems()
            {
                var exemptedItemsList = new List<string> { "az", "77", "67" };
                const int expectedExemptItemCount = 3;

                FlattenCollection(_itemsList).Count(i => i.IsExempt).Should().Be(0);

                Exemptions.MarkExempted(_itemsList, exemptedItemsList);

                var flat = FlattenCollection(_itemsList);
                flat.Count(i => i.IsExempt).Should().Be(expectedExemptItemCount);
                flat.First(i => i.Sku == "1").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "A").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "2").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "99").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "76").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "77").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "67").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "131").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "313").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "az").IsExempt.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void SingleSubSubItem()
            {
                var exemptedItemsList = new List<string> { "131" };
                const int expectedExemptItemCount = 1;

                FlattenCollection(_itemsList).Count(i => i.IsExempt).Should().Be(0);

                Exemptions.MarkExempted(_itemsList, exemptedItemsList);

                var flat = FlattenCollection(_itemsList);
                flat.Count(i => i.IsExempt).Should().Be(expectedExemptItemCount);
                flat.First(i => i.Sku == "1").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "A").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "2").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "99").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "76").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "77").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "67").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "131").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "313").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "az").IsExempt.Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void MultipleSubSubItems()
            {
                var exemptedItemsList = new List<string> { "131", "313" };
                const int expectedExemptItemCount = 2;

                FlattenCollection(_itemsList).Count(i => i.IsExempt).Should().Be(0);

                Exemptions.MarkExempted(_itemsList, exemptedItemsList);

                var flat = FlattenCollection(_itemsList);
                flat.Count(i => i.IsExempt).Should().Be(expectedExemptItemCount);
                flat.First(i => i.Sku == "1").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "A").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "2").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "99").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "76").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "77").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "67").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "131").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "313").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "az").IsExempt.Should().BeFalse();
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void ItemAlreadySetExempt()
            {
                var itemsList = new List<IExemptableItem>(_itemsList)
                    {
                        new ItemTestClass("999", 1.54m, true),
                    };

                var exemptedItemsList = new List<string> { "131", "313" };
                const int expectedExemptItemCount = 3;

                FlattenCollection(itemsList).Count(i => i.IsExempt).Should().Be(1);

                Exemptions.MarkExempted(itemsList, exemptedItemsList);

                var flat = FlattenCollection(itemsList);
                flat.Count(i => i.IsExempt).Should().Be(expectedExemptItemCount);
                flat.First(i => i.Sku == "1").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "A").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "2").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "99").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "76").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "77").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "67").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "131").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "313").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "az").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "999").IsExempt.Should().BeTrue();
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void SubItemAlreadySetExempt()
            {
                var itemsList = new List<IExemptableItem>(_itemsList)
                    {
                        new ItemTestClass("888", 5.99m, false, new Collection<IExemptableItem>()
                            {
                                new ItemTestClass("999", 1.54m, true),

                            }),
                    };

                var exemptedItemsList = new List<string> { "131", "313" };
                const int expectedExemptItemCount = 3;

                FlattenCollection(itemsList).Count(i => i.IsExempt).Should().Be(1);

                Exemptions.MarkExempted(itemsList, exemptedItemsList);

                var flat = FlattenCollection(itemsList);
                flat.Count(i => i.IsExempt).Should().Be(expectedExemptItemCount);
                flat.First(i => i.Sku == "1").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "A").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "2").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "99").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "76").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "77").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "67").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "131").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "313").IsExempt.Should().BeTrue();
                flat.First(i => i.Sku == "az").IsExempt.Should().BeFalse();
                flat.First(i => i.Sku == "999").IsExempt.Should().BeTrue();
            }
        }

        [TestClass]
        public class SumItemTreeTests
        {
            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void NullItem()
            {
                IEnumerable<IExemptableItem> items = new Collection<IExemptableItem>
                    {
                        (IExemptableItem)null,
                    };

                const decimal expectedExemptionTotal = 0m;

                Exemptions.Sum(items).Should().Be(expectedExemptionTotal);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void NullItemList()
            {
                IEnumerable<IExemptableItem> items = null;
                const decimal expectedExemptionTotal = 0m;

                Exemptions.Sum(items).Should().Be(expectedExemptionTotal);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void NullItemInList()
            {
                IEnumerable<IExemptableItem> itemsList = new Collection<IExemptableItem>()
                    {
                        new ItemTestClass("ZzZ", 5.43m),
                        new ItemTestClass("909", 7.89m, false, new Collection<IExemptableItem>()
                            {
                                new ItemTestClass("44", 8.11m),
                                new ItemTestClass("888", 0.40m),
                                new ItemTestClass("3141", 0.99m),
                                new ItemTestClass("67", 0.50m, false, new Collection<IExemptableItem>()
                                    {
                                        new ItemTestClass("777", 0.30m),
                                        new ItemTestClass("LMNO", 0.10m),
                                    }),
                                new ItemTestClass("odd", 4.56m),
                            }),
                        new ItemTestClass("6000", 1m),
                    };

                List<IExemptableItem> listWithNull = new List<IExemptableItem>(itemsList)
                    {
                        (IExemptableItem) null,
                        new ItemTestClass("za", 3.41m)
                    };

                Exemptions.Sum(listWithNull).Should().Be(0m);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void EmptyItemList()
            {
                List<IExemptableItem> empty = new List<IExemptableItem>();

                Exemptions.Sum(empty).Should().Be(0);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void SingleTopLevelItem()
            {
                var itemsList = new Collection<IExemptableItem>()
                    {
                        new ItemTestClass("ZzZ", 5.43m),
                        new ItemTestClass("909", 7.89m, true, new Collection<IExemptableItem>()
                            {
                                new ItemTestClass("44", 8.11m),
                                new ItemTestClass("888", 0.40m),
                                new ItemTestClass("3141", 0.99m),
                                new ItemTestClass("67", 0.50m, false, new Collection<IExemptableItem>()
                                    {
                                        new ItemTestClass("777", 0.30m),
                                        new ItemTestClass("LMNO", 0.10m),
                                    }),
                                new ItemTestClass("odd", 4.56m),
                            }),
                        new ItemTestClass("6000", 1m),
                    };

                const decimal expectedExemptionTotal = 7.89m;

                Exemptions.Sum(itemsList).Should().Be(expectedExemptionTotal);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void MultipleItems()
            {
                var itemsList = new Collection<IExemptableItem>()
                    {
                        new ItemTestClass("ZzZ", 5.43m, true),
                        new ItemTestClass("909", 7.89m, false, new Collection<IExemptableItem>()
                            {
                                new ItemTestClass("44", 8.11m),
                                new ItemTestClass("888", 0.40m),
                                new ItemTestClass("3141", 0.99m, true),
                                new ItemTestClass("67", 0.50m, false, new Collection<IExemptableItem>()
                                    {
                                        new ItemTestClass("777", 0.30m),
                                        new ItemTestClass("LMNO", 0.10m, true),
                                    }),
                                new ItemTestClass("odd", 4.56m),
                            }),
                        new ItemTestClass("6000", 1m),
                    };

                const decimal expectedExemptionTotal = 6.52m;

                Exemptions.Sum(itemsList).Should().Be(expectedExemptionTotal);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void MultipleTopLevelItems()
            {
                var itemsList = new Collection<IExemptableItem>()
                    {
                        new ItemTestClass("ZzZ", 5.43m),
                        new ItemTestClass("909", 7.89m, true, new Collection<IExemptableItem>()
                            {
                                new ItemTestClass("44", 8.11m),
                                new ItemTestClass("888", 0.40m),
                                new ItemTestClass("3141", 0.99m),
                                new ItemTestClass("67", 0.50m, false, new Collection<IExemptableItem>()
                                    {
                                        new ItemTestClass("777", 0.30m),
                                        new ItemTestClass("LMNO", 0.10m),
                                    }),
                                new ItemTestClass("odd", 4.56m),
                            }),
                        new ItemTestClass("6000", 1m, true, null),
                    };


                const decimal expectedExemptionTotal = 8.89m;

                Exemptions.Sum(itemsList).Should().Be(expectedExemptionTotal);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void SingleSubItem()
            {
                var itemsList = new Collection<IExemptableItem>()
                    {
                        new ItemTestClass("ZzZ", 5.43m),
                        new ItemTestClass("909", 7.89m, false, new Collection<IExemptableItem>()
                            {
                                new ItemTestClass("44", 8.11m),
                                new ItemTestClass("888", 0.40m),
                                new ItemTestClass("3141", 0.99m),
                                new ItemTestClass("67", 0.50m, false, new Collection<IExemptableItem>()
                                    {
                                        new ItemTestClass("777", 0.30m),
                                        new ItemTestClass("LMNO", 0.10m),
                                    }),
                                new ItemTestClass("odd", 4.56m, true, null),
                            }),
                        new ItemTestClass("6000", 1m),
                    };

                const decimal expectedExemptionTotal = 4.56m;

                Exemptions.Sum(itemsList).Should().Be(expectedExemptionTotal);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void MultipleSubItems()
            {
                var itemsList = new Collection<IExemptableItem>()
                    {
                        new ItemTestClass("ZzZ", 5.43m),
                        new ItemTestClass("909", 7.89m, false, new Collection<IExemptableItem>()
                            {
                                new ItemTestClass("44", 8.11m),
                                new ItemTestClass("888", 0.40m, true, null),
                                new ItemTestClass("3141", 0.99m, true, null),
                                new ItemTestClass("67", 0.50m, false, new Collection<IExemptableItem>()
                                    {
                                        new ItemTestClass("777", 0.30m),
                                        new ItemTestClass("LMNO", 0.10m),
                                    }),
                                new ItemTestClass("odd", 4.56m, true, null),
                            }),
                        new ItemTestClass("6000", 1m),
                    };

                const decimal expectedExemptionTotal = 5.95m;

                Exemptions.Sum(itemsList).Should().Be(expectedExemptionTotal);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void SingleSubSubItem()
            {
                var itemsList = new Collection<IExemptableItem>()
                    {
                        new ItemTestClass("ZzZ", 5.43m),
                        new ItemTestClass("909", 7.89m, false, new Collection<IExemptableItem>()
                            {
                                new ItemTestClass("44", 8.11m),
                                new ItemTestClass("888", 0.40m),
                                new ItemTestClass("3141", 0.99m),
                                new ItemTestClass("67", 0.50m, false, new Collection<IExemptableItem>()
                                    {
                                        new ItemTestClass("777", 0.30m),
                                        new ItemTestClass("LMNO", 0.10m, true, null),
                                    }),
                                new ItemTestClass("odd", 4.56m),
                            }),
                        new ItemTestClass("6000", 1m),
                    };

                const decimal expectedExemptionTotal = 0.10m;

                Exemptions.Sum(itemsList).Should().Be(expectedExemptionTotal);
            }
            
            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void MultipleSubSubItems()
            {
                var itemsList = new Collection<IExemptableItem>()
                    {
                        new ItemTestClass("ZzZ", 5.43m),
                        new ItemTestClass("909", 7.89m, false, new Collection<IExemptableItem>()
                            {
                                new ItemTestClass("44", 8.11m),
                                new ItemTestClass("888", 0.40m),
                                new ItemTestClass("3141", 0.99m),
                                new ItemTestClass("67", 0.50m, false, new Collection<IExemptableItem>()
                                    {
                                        new ItemTestClass("777", 0.30m, true),
                                        new ItemTestClass("LMNO", 0.10m, true, null),
                                    }),
                                new ItemTestClass("odd", 4.56m),
                            }),
                        new ItemTestClass("6000", 1m),
                    };

                const decimal expectedExemptionTotal = 0.40m;

                Exemptions.Sum(itemsList).Should().Be(expectedExemptionTotal);
            }
        }
    }
}
