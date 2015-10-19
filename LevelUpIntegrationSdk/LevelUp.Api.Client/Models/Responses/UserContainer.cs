//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="UserContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class to hold the results from the user end point
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UserContainer
    {
        public UserContainer()
        {
            CustomAttributes = new Dictionary<string, string>();
        }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "born_at")]
        public string BornAt { get; set; }

        [JsonProperty(PropertyName = "custom_attributes")]
        public Dictionary<string, string> CustomAttributes { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; set; }

        [JsonProperty(PropertyName = "global_credit_amount")]
        public int GlobalCreditAmount { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "merchants_visited_count")]
        public int MerchantsVisitedCount { get; set; }

        [JsonProperty(PropertyName = "orders_count")]
        public int OrdersCount { get; set; }

        [JsonProperty(PropertyName = "terms_accepted_at")]
        public string TermsAcceptedAt { get; set; }

        [JsonProperty(PropertyName = "total_savings_amount")]
        public int TotalSavingsAmount { get; set; }

        public override string ToString()
        {
            StringBuilder customAttributesDetails = new StringBuilder();

            if (CustomAttributes == null || CustomAttributes.Count == 0)
            {
                customAttributesDetails.Append("NONE");
            }
            else
            {
                foreach (KeyValuePair<string, string> pair in CustomAttributes)
                {
                    customAttributesDetails.AppendFormat("{{{0}:{1}}}", pair.Key, pair.Value);
                }
            }

            return string.Format(new CultureInfo("en-US"),
                                 "Id: {0}{1}" +
                                 "BornAt: {2}{1}" +
                                 "CustomAttributes: {3}{1}" +
                                 "Email: {4}{1}" +
                                 "FirstName: {5}{1}" +
                                 "Gender: {6}{1}" +
                                 "GlobalCreditAmount: {7}{1}" +
                                 "MerchantsVisitedCount: {8}{1}" +
                                 "OrdersCount: {9}{1}" +
                                 "TermsAcceptedAt: {10}{1}" +
                                 "TotalSavingsAmount: {11}{1}",
                                 Id,
                                 Environment.NewLine,
                                 BornAt,
                                 customAttributesDetails,
                                 Email,
                                 FirstName,
                                 Gender,
                                 GlobalCreditAmount,
                                 MerchantsVisitedCount,
                                 OrdersCount,
                                 TermsAcceptedAt,
                                 TotalSavingsAmount);
        }
    }
}
