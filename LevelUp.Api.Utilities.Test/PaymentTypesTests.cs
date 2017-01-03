using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Utilities.Test
{
    [TestClass]
    public class PaymentTypesTests
    {
        [TestMethod]
        public void FlagsEnum_None()
        {
            PaymentTypes none = PaymentTypes.None;

            none.Should().Be(PaymentTypes.None);
            none.Should().NotBe(PaymentTypes.Discount);
            none.Should().NotBe(PaymentTypes.LevelUp);
            none.Should().NotBe(PaymentTypes.GiftCard);
            none.Should().NotBe(PaymentTypes.All);
            none.Should().NotBe(PaymentTypes.Discount | PaymentTypes.LevelUp);
            none.Should().NotBe(PaymentTypes.Discount | PaymentTypes.GiftCard);
            none.Should().NotBe(PaymentTypes.GiftCard | PaymentTypes.LevelUp);

            (none & PaymentTypes.Discount).Should().Be(PaymentTypes.None);
            (none & PaymentTypes.LevelUp).Should().Be(PaymentTypes.None);
            (none & PaymentTypes.GiftCard).Should().Be(PaymentTypes.None);
            (none & (PaymentTypes.Discount | PaymentTypes.LevelUp)).Should().Be(PaymentTypes.None);
            (none & (PaymentTypes.Discount | PaymentTypes.GiftCard)).Should().Be(PaymentTypes.None);
            (none & (PaymentTypes.GiftCard | PaymentTypes.LevelUp)).Should().Be(PaymentTypes.None);
            (none & PaymentTypes.All).Should().Be(PaymentTypes.None);

            (none | PaymentTypes.Discount).Should().Be(PaymentTypes.Discount);
            (none | PaymentTypes.LevelUp).Should().Be(PaymentTypes.LevelUp);
            (none | PaymentTypes.GiftCard).Should().Be(PaymentTypes.GiftCard);
            (none | (PaymentTypes.Discount & PaymentTypes.LevelUp)).Should().Be(PaymentTypes.None);
            (none | (PaymentTypes.Discount & PaymentTypes.GiftCard)).Should().Be(PaymentTypes.None);
            (none | (PaymentTypes.LevelUp & PaymentTypes.GiftCard)).Should().Be(PaymentTypes.None);
            (none | PaymentTypes.All).Should().Be(PaymentTypes.All);

            foreach (var value in Enum.GetValues(typeof(PaymentTypes)))
            {
                PaymentTypes typeValue = (PaymentTypes)value;
                (none & typeValue).Should().Be(PaymentTypes.None);
            }
        }

        [TestMethod]
        public void FlagsEnum_All()
        {
            PaymentTypes all = PaymentTypes.All;

            all.Should().Be(PaymentTypes.Discount | PaymentTypes.GiftCard | PaymentTypes.LevelUp);
            all.Should().NotBe(PaymentTypes.None);
            all.Should().NotBe(PaymentTypes.LevelUp);
            all.Should().NotBe(PaymentTypes.Discount);
            all.Should().NotBe(PaymentTypes.GiftCard);
            all.Should().NotBe(PaymentTypes.LevelUp | PaymentTypes.Discount);
            all.Should().NotBe(PaymentTypes.LevelUp | PaymentTypes.GiftCard);
            all.Should().NotBe(PaymentTypes.GiftCard | PaymentTypes.Discount);

            (all & PaymentTypes.Discount).Should().Be(PaymentTypes.Discount);
            (all & PaymentTypes.LevelUp).Should().Be(PaymentTypes.LevelUp);
            (all & PaymentTypes.GiftCard).Should().Be(PaymentTypes.GiftCard);
            (all & (PaymentTypes.Discount | PaymentTypes.LevelUp)).Should().Be(PaymentTypes.Discount | PaymentTypes.LevelUp);
            (all & (PaymentTypes.Discount | PaymentTypes.GiftCard)).Should().Be(PaymentTypes.Discount | PaymentTypes.GiftCard);
            (all & (PaymentTypes.GiftCard | PaymentTypes.LevelUp)).Should().Be(PaymentTypes.GiftCard | PaymentTypes.LevelUp);
            (all & PaymentTypes.None).Should().Be(PaymentTypes.None);
            (all & PaymentTypes.All).Should().Be(PaymentTypes.All);

            (all | PaymentTypes.Discount).Should().Be(PaymentTypes.All);
            (all | PaymentTypes.LevelUp).Should().Be(PaymentTypes.All);
            (all | PaymentTypes.GiftCard).Should().Be(PaymentTypes.All);
            (all | PaymentTypes.None).Should().Be(PaymentTypes.All);
            (all | PaymentTypes.All).Should().Be(PaymentTypes.All);

            (all ^ PaymentTypes.Discount).Should().Be(PaymentTypes.LevelUp | PaymentTypes.GiftCard);
            (all ^ PaymentTypes.GiftCard).Should().Be(PaymentTypes.LevelUp | PaymentTypes.Discount);
            (all ^ PaymentTypes.LevelUp).Should().Be(PaymentTypes.Discount | PaymentTypes.GiftCard);

            (all ^ (PaymentTypes.Discount | PaymentTypes.GiftCard)).Should().Be(PaymentTypes.LevelUp);
            (all ^ (PaymentTypes.Discount | PaymentTypes.LevelUp)).Should().Be(PaymentTypes.GiftCard);
            (all ^ (PaymentTypes.LevelUp | PaymentTypes.GiftCard)).Should().Be(PaymentTypes.Discount);

            foreach (var value in Enum.GetValues(typeof (PaymentTypes)))
            {
                PaymentTypes typeValue = (PaymentTypes) value;

                (all & typeValue).Should().Be(typeValue);
            }
        }

        [TestMethod]
        public void FlagsEnum_Discount()
        {
            PaymentTypes discount = PaymentTypes.Discount;

            discount.Should().Be(PaymentTypes.Discount);
            discount.Should().NotBe(PaymentTypes.LevelUp);
            discount.Should().NotBe(PaymentTypes.GiftCard);
            discount.Should().NotBe(PaymentTypes.All);
            discount.Should().NotBe(PaymentTypes.None);
            discount.Should().NotBe(PaymentTypes.Discount | PaymentTypes.LevelUp);
            discount.Should().NotBe(PaymentTypes.Discount | PaymentTypes.GiftCard);
            discount.Should().NotBe(PaymentTypes.GiftCard | PaymentTypes.LevelUp);

            (discount & PaymentTypes.Discount).Should().Be(PaymentTypes.Discount);
            (discount & PaymentTypes.LevelUp).Should().Be(PaymentTypes.None);
            (discount & PaymentTypes.GiftCard).Should().Be(PaymentTypes.None);
            (discount & (PaymentTypes.Discount | PaymentTypes.LevelUp)).Should().Be(PaymentTypes.Discount);
            (discount & (PaymentTypes.Discount | PaymentTypes.GiftCard)).Should().Be(PaymentTypes.Discount);
            (discount & (PaymentTypes.GiftCard | PaymentTypes.LevelUp)).Should().Be(PaymentTypes.None);
            (discount & PaymentTypes.None).Should().Be(PaymentTypes.None);
            (discount & PaymentTypes.All).Should().Be(PaymentTypes.Discount);

            (discount | PaymentTypes.Discount).Should().Be(PaymentTypes.Discount);
            (discount | PaymentTypes.LevelUp).Should().Be(PaymentTypes.LevelUp | PaymentTypes.Discount);
            (discount | PaymentTypes.GiftCard).Should().Be(PaymentTypes.GiftCard | PaymentTypes.Discount);
            (discount | PaymentTypes.None).Should().Be(PaymentTypes.Discount);
            (discount | PaymentTypes.All).Should().Be(PaymentTypes.All);
        }

        [TestMethod]
        public void FlagsEnum_LevelUp()
        {
            PaymentTypes levelUp = PaymentTypes.LevelUp;

            levelUp.Should().Be(PaymentTypes.LevelUp);
            levelUp.Should().NotBe(PaymentTypes.Discount);
            levelUp.Should().NotBe(PaymentTypes.GiftCard);
            levelUp.Should().NotBe(PaymentTypes.All);
            levelUp.Should().NotBe(PaymentTypes.None);
            levelUp.Should().NotBe(PaymentTypes.Discount | PaymentTypes.LevelUp);
            levelUp.Should().NotBe(PaymentTypes.Discount | PaymentTypes.GiftCard);
            levelUp.Should().NotBe(PaymentTypes.GiftCard | PaymentTypes.LevelUp);

            (levelUp & PaymentTypes.Discount).Should().Be(PaymentTypes.None);
            (levelUp & PaymentTypes.LevelUp).Should().Be(PaymentTypes.LevelUp);
            (levelUp & PaymentTypes.GiftCard).Should().Be(PaymentTypes.None);
            (levelUp & (PaymentTypes.Discount | PaymentTypes.LevelUp)).Should().Be(PaymentTypes.LevelUp);
            (levelUp & (PaymentTypes.Discount | PaymentTypes.GiftCard)).Should().Be(PaymentTypes.None);
            (levelUp & (PaymentTypes.GiftCard | PaymentTypes.LevelUp)).Should().Be(PaymentTypes.LevelUp);
            (levelUp & PaymentTypes.All).Should().Be(PaymentTypes.LevelUp);

            (levelUp | PaymentTypes.Discount).Should().Be(PaymentTypes.Discount | PaymentTypes.LevelUp);
            (levelUp | PaymentTypes.LevelUp).Should().Be(PaymentTypes.LevelUp);
            (levelUp | PaymentTypes.GiftCard).Should().Be(PaymentTypes.GiftCard | PaymentTypes.LevelUp);
            (levelUp | PaymentTypes.None).Should().Be(PaymentTypes.LevelUp);
            (levelUp | PaymentTypes.All).Should().Be(PaymentTypes.All);
        }

        [TestMethod]
        public void FlagsEnum_GiftCard()
        {
            PaymentTypes gift = PaymentTypes.GiftCard;

            gift.Should().Be(PaymentTypes.GiftCard);
            gift.Should().NotBe(PaymentTypes.Discount);
            gift.Should().NotBe(PaymentTypes.LevelUp);
            gift.Should().NotBe(PaymentTypes.All);
            gift.Should().NotBe(PaymentTypes.None);
            gift.Should().NotBe(PaymentTypes.Discount | PaymentTypes.LevelUp);
            gift.Should().NotBe(PaymentTypes.Discount | PaymentTypes.GiftCard);
            gift.Should().NotBe(PaymentTypes.GiftCard | PaymentTypes.LevelUp);

            (gift & PaymentTypes.Discount).Should().Be(PaymentTypes.None);
            (gift & PaymentTypes.LevelUp).Should().Be(PaymentTypes.None);
            (gift & PaymentTypes.GiftCard).Should().Be(PaymentTypes.GiftCard);
            (gift & (PaymentTypes.Discount | PaymentTypes.LevelUp)).Should().Be(PaymentTypes.None);
            (gift & (PaymentTypes.Discount | PaymentTypes.GiftCard)).Should().Be(PaymentTypes.GiftCard);
            (gift & (PaymentTypes.GiftCard | PaymentTypes.LevelUp)).Should().Be(PaymentTypes.GiftCard);
            (gift & PaymentTypes.All).Should().Be(PaymentTypes.GiftCard);

            (gift | PaymentTypes.Discount).Should().Be(PaymentTypes.Discount | PaymentTypes.GiftCard);
            (gift | PaymentTypes.LevelUp).Should().Be(PaymentTypes.LevelUp | PaymentTypes.GiftCard);
            (gift | PaymentTypes.GiftCard).Should().Be(PaymentTypes.GiftCard);
            (gift | PaymentTypes.None).Should().Be(PaymentTypes.GiftCard);
            (gift | PaymentTypes.All).Should().Be(PaymentTypes.All);
        }
    }
}
