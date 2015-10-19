using FluentAssertions;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test
{
    [TestClass]
    public class TestLevelUpEndpoints
    {
        private const string v14BaseUri = "https://api.thelevelup.com/v14/";
        
        private readonly LevelUpEndpoints _endpoints = new LevelUpEndpoints(LevelUpEnvironment.Production);

        [TestMethod]
        public void AuthenticationEndpoint()
        {
            _endpoints.Authentication().ShouldBeEquivalentTo(string.Format("{0}access_tokens", v14BaseUri));
        }

        [TestMethod]
        public void CreditCardsEndpoint()
        {
            const int creditCardId = 123;
            const string creditCards = "credit_cards";
            _endpoints.CreditCards().ShouldBeEquivalentTo(string.Format("{0}{1}", v14BaseUri, creditCards));
            _endpoints.CreditCards(creditCardId).ShouldBeEquivalentTo(string.Format("{0}{1}/{2}", v14BaseUri, creditCards, creditCardId));
        }

        [TestMethod]
        public void ContributionTargetsEndpoint()
        {
            const string contributionId = "321";
            const string path = "contributions/targets";
            _endpoints.ContributionTargets().ShouldBeEquivalentTo(string.Format("{0}{1}", v14BaseUri, path));
            _endpoints.ContributionTargetDetails(contributionId).ShouldBeEquivalentTo(string.Format("{0}{1}/{2}", v14BaseUri, path, contributionId));

        }

        [TestMethod]
        public void DetachedRefundEndpoint()
        {
            _endpoints.DetachedRefund().ShouldBeEquivalentTo(string.Format("{0}detached_refunds", v14BaseUri));
        }

        [TestMethod]
        public void GiftCardAddValueEndpoint()
        {
            const int merchantId = 5555;
            string expected = string.Format("https://api.thelevelup.com/v15/merchants/{0}/gift_card_value_additions",
                                            merchantId);
            _endpoints.GiftCardAddValue(merchantId).ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void GiftCardDestroyValueEndpoint()
        {
            const int merchantId = 4444;
            string expected = string.Format("https://api.thelevelup.com/v15/merchants/{0}/gift_card_value_removals",
                                            merchantId);
            _endpoints.GiftCardRemoveValue(merchantId).ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void LocationsEndpoint()
        {
            const int locationId = 54321;
            _endpoints.Locations(locationId).ShouldBeEquivalentTo(string.Format("{0}merchants/{1}/locations", v14BaseUri, locationId));
        }

        [TestMethod]
        public void LocationDetailsEndpoint()
        {
            const int locationId = 12345;
            _endpoints.LocationDetails(locationId).ShouldBeEquivalentTo(string.Format("{0}locations/{1}", v14BaseUri, locationId));
        }

        [TestMethod]
        public void LoyaltyEndpoint()
        {
            const int merchantId = 12345;
            _endpoints.Loyalty(merchantId).ShouldBeEquivalentTo(string.Format("{0}merchants/{1}/loyalty", v14BaseUri, merchantId));
        }

        [TestMethod]
        public void MerchantCreditEndpoint()
        {
            const int locationId = 12345;
            const string qrCode = "LUabcdefg1234567890LU";
            string expected = string.Format("{0}locations/{1}/merchant_funded_credit?payment_token_data={2}",
                                            v14BaseUri,
                                            locationId,
                                            qrCode);
            _endpoints.MerchantCredit(locationId, qrCode).ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void OrderEndpoint()
        {
            _endpoints.Order().ShouldBeEquivalentTo(string.Format("{0}orders", v14BaseUri));
        }

        [TestMethod]
        public void OrderDetailsEndpoint()
        {
            const int merchantId = 12345;
            const string orderId = "abcdefg1234567890gfedcba";
            string expected = string.Format("{0}merchants/{1}/orders/{2}",
                                            v14BaseUri,
                                            merchantId,
                                            orderId);
            _endpoints.OrderDetails(merchantId, orderId).ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void OrdersByLocationEndpoint()
        {
            const int locationId = 12345;
            const string ordersByLocationBase = "{0}locations/{1}/orders";
            string expected0 = string.Format(ordersByLocationBase,
                                v14BaseUri,
                                locationId);

            string expected1 = string.Format(ordersByLocationBase + "?page=2",
                                             v14BaseUri,
                                             locationId);

            _endpoints.OrdersByLocation(locationId).ShouldBeEquivalentTo(expected0);
            _endpoints.OrdersByLocation(locationId, 2).ShouldBeEquivalentTo(expected1);
            _endpoints.OrdersByLocation(locationId, 1).ShouldBeEquivalentTo(expected0);
            _endpoints.OrdersByLocation(locationId, -1).ShouldBeEquivalentTo(expected0);
        }

        [TestMethod]
        public void PasswordsEndpoint()
        {
            _endpoints.Passwords().ShouldBeEquivalentTo(string.Format("{0}passwords", v14BaseUri));
        }

        [TestMethod]
        public void PaymentTokenEndpoint()
        {
            _endpoints.PaymentToken().ShouldBeEquivalentTo(string.Format("{0}payment_token", v14BaseUri));
        }

        [TestMethod]
        public void RefundEndpoint()
        {
            const string orderId = "abcdefg1234567890gfedcba";
            _endpoints.Refund(orderId).ShouldBeEquivalentTo(string.Format("{0}orders/{1}/refund", v14BaseUri, orderId));
        }

        [TestMethod]
        public void UserEndpoint()
        {
            const int userId = 314159;
            _endpoints.User(userId).ShouldBeEquivalentTo(string.Format("{0}users/{1}", v14BaseUri, userId));
        }

        [TestMethod]
        public void UsersEndpoint()
        {
            _endpoints.Users().ShouldBeEquivalentTo(string.Format("{0}users", v14BaseUri));
        }

        [TestMethod]
        public void UserAddressesEndpoint()
        {
            _endpoints.UserAddresses().ShouldBeEquivalentTo(string.Format("{0}user_addresses", v14BaseUri));
        }
    }
}
