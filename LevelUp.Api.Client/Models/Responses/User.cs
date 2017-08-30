#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="User.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using LevelUp.Api.Utilities;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp user
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("user")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class User : IResponse
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private User()
        {
            CustomAttributesInternal = new Dictionary<string, string>();
        }

        public User(int id, Dictionary<string, string> customAttributes, string email, string firstName, string gender,
            int globalCreditAmount, string lastName, int merchantsVisitedCount, int ordersCount,
            int totalSavingsAmount, string termsAcceptedAt, string bornAt)
        {
            Id = id;
            CustomAttributesInternal = customAttributes; Email = email;
            FirstName = firstName; 
            Gender = gender;
            GlobalCreditAmount = globalCreditAmount;
            LastName = lastName;
            MerchantsVisitedCount = merchantsVisitedCount;
            OrdersCount = ordersCount;
            TotalSavingsAmount = totalSavingsAmount;
            TermsAcceptedAtInternal = termsAcceptedAt;
            BornAtInternal = bornAt;
        }

        /// <summary>
        /// The user's ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; private set; }

        /// <summary>
        /// Arbitrary key-value data to associate with this user
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, string> CustomAttributes
        {
            get { return (CustomAttributesInternal != null) ? new Dictionary<string, string>(CustomAttributesInternal) : null; }
        }

        [JsonProperty(PropertyName = "custom_attributes")]
        private Dictionary<string, string> CustomAttributesInternal { get; set; }

        /// <summary>
        /// The user's email address
        /// </summary>
        [JsonProperty(PropertyName = "email")]
        public string Email { get; private set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; private set; }

        /// <summary>
        /// The user's gender
        /// </summary>
        [JsonProperty(PropertyName = "gender")]
        public string Gender { get; private set; }

        /// <summary>
        /// The amount of global credit the user currently has
        /// </summary>
        [JsonProperty(PropertyName = "global_credit_amount")]
        public int GlobalCreditAmount { get; private set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; private set; }

        /// <summary>
        /// The number of merchants the user has visited
        /// </summary>
        [JsonProperty(PropertyName = "merchants_visited_count")]
        public int MerchantsVisitedCount { get; private set; }

        /// <summary>
        /// The number of orders the user has placed
        /// </summary>
        [JsonProperty(PropertyName = "orders_count")]
        public int OrdersCount { get; private set; }

        /// <summary>
        /// The amount of money the user has saved since joining LevelUp
        /// </summary>
        [JsonProperty(PropertyName = "total_savings_amount")]
        public int TotalSavingsAmount { get; private set; }

        /// <summary>
        /// The date & time the user accepted the LevelUp terms at
        /// </summary>
        [JsonIgnore]
        public virtual DateTime? TermsAcceptedAt
        {
            get { return ParseDate(TermsAcceptedAtInternal); }
        }

        [JsonProperty(PropertyName = "terms_accepted_at")]
        private string TermsAcceptedAtInternal { get; set; }

        /// <summary>
        /// The user's birthday
        /// </summary>
        [JsonIgnore]
        public virtual DateTime? BornAt
        {
            get { return ParseDate(BornAtInternal); }
        }

        [JsonProperty(PropertyName = "born_at")]
        private string BornAtInternal { get; set; }

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

            return string.Format(Constants.EnUsCulture,
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

        private static DateTime? ParseDate(string dateStr)
        {
            DateTime parseDate;
            if (DateTime.TryParseExact(dateStr, Constants.Iso8601DateTimeFormat, Constants.EnUsCulture, 
                DateTimeStyles.AssumeLocal, out parseDate))
            {
                return parseDate;
            }

            DateTimeOffset offsetParseDate;
            if (DateTimeOffset.TryParseExact(dateStr, Constants.Iso8601DateTimeOffsetFormat, Constants.EnUsCulture, 
                DateTimeStyles.AssumeLocal, out offsetParseDate))
            {
                return offsetParseDate.DateTime;
            }
            
            return new DateTime?();
        }
    }
}
