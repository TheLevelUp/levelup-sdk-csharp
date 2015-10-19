//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpEndpoints.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2014 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using System;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client
{
    public class LevelUpEndpoints
    {
        private readonly LevelUpUriBuilder _uriBuilder;

        public LevelUpEndpoints() : this(LevelUpEnvironment.Production) { }

        public LevelUpEndpoints(LevelUpEnvironment environment)
        {
            _uriBuilder = new LevelUpUriBuilder(environment);
        }

        public LevelUpEndpoints(string baseUri)
        {
            _uriBuilder = new LevelUpUriBuilder(baseUri);
        }

        public virtual string Authentication()
        {
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath("access_tokens").Build();
        }

        public virtual string CreditCards()
        {
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath("credit_cards").Build();
        }

        public virtual string CreditCards(int creditCardId)
        {
            string path = string.Format("credit_cards/{0}", creditCardId);
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath(path).Build();
        }

        public virtual string ContributionTargets()
        {
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath("contributions/targets").Build();
        }

        public virtual string ContributionTargetDetails(string contributionTargetId)
        {
            string path = string.Format("contributions/targets/{0}", contributionTargetId);
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath(path).Build();
        }

        public virtual string DetachedRefund()
        {
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath("detached_refunds").Build();
        }

        public virtual string GiftCardAddValue(int merchantId)
        {
            string path = string.Format("merchants/{0}/gift_card_value_additions", merchantId);
            //NOTE: This is a v15 endpoint! No v14 endpoint for gift card add value exists at this time (06/26/2015)
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v15).SetPath(path).Build();
        }

        public virtual string GiftCardRemoveValue(int merchantId)
        {
            string path = string.Format("merchants/{0}/gift_card_value_removals", merchantId);
            //NOTE: This is a v15 endpoint! No v14 endpoint for gift card remove value exists at this time (06/26/2015)
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v15).SetPath(path).Build();
        }

        public virtual string Locations(int merchantId)
        {
            string path = string.Format("merchants/{0}/locations", merchantId);
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath(path).Build();
        }

        public virtual string LocationDetails(int locationId)
        {
            string path = string.Format("locations/{0}", locationId);
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath(path).Build();
        }

        public virtual string Loyalty(int merchantId)
        {
            string path = string.Format("merchants/{0}/loyalty", merchantId);
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath(path).Build();
        }

        public virtual string MerchantCredit(int locationId, string paymentToken)
        {
            string path = string.Format("locations/{0}/merchant_funded_credit", locationId);
            _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath(path).AppendQuery("payment_token_data",
                                                                                       paymentToken);
            return _uriBuilder.Build();
        }

        public virtual string Order()
        {
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath("orders").Build();
        }

        public virtual string OrderDetails(int merchantId, string orderIdentifier)
        {
            string path = string.Format("merchants/{0}/orders/{1}",
                                        merchantId,
                                        orderIdentifier);
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath(path).Build();
        }

        public virtual string OrdersByLocation(int locationId, int pageNumber = 1)
        {
            string path = string.Format("locations/{0}/orders", locationId);

            _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath(path);

            if (pageNumber > 1)
            {
                _uriBuilder.AppendQuery("page", pageNumber.ToString());
            }

            return _uriBuilder.Build();
        }

        public virtual string Passwords()
        {
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath("passwords").Build();
        }

        public virtual string PaymentToken()
        {
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath("payment_token").Build();
        }

        public virtual string Refund(string orderIdentifier)
        {
            string path = string.Format("orders/{0}/refund", orderIdentifier);
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath(path).Build();
        }

        public virtual string User(int userId)
        {
            string path = string.Format("users/{0}", userId);
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath(path).Build();
        }

        public virtual string Users()
        {
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath("users").Build();
        }

        public virtual string UserAddresses()
        {
            return _uriBuilder.SetApiVersion(LevelUpApiVersion.v14).SetPath("user_addresses").Build();
        }

        internal string Host { get { return _uriBuilder.Host; } }

        internal string BaseUri
        {
            get
            {
                return string.Format("{0}://{1}/",
                                     _uriBuilder.Scheme,
                                     Host);
            }
        }
    }
}
