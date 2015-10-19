//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="User.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp user
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class User
    {
        public User()
        {
            // keep for serialization purposes
        }

        /// <summary>
        /// The user's ID
        /// </summary>
        public virtual int Id { get { return UserContainer.Id; } }

        /// <summary>
        /// The user's birthday
        /// </summary>
        public virtual DateTime? BornAt
        {
            get { return Utils.TryParseDate(UserContainer.BornAt, Constants.Iso8601DateFormat); }
        }

        /// <summary>
        /// Arbitrary key-value data to associate with this user
        /// </summary>
        public virtual Dictionary<string, string> CustomAttributes
        {
            get { return UserContainer.CustomAttributes ?? new Dictionary<string, string>(); }
        }

        /// <summary>
        /// The user's email address
        /// </summary>
        public virtual string Email { get { return UserContainer.Email; } }

        /// <summary>
        /// The user's first name
        /// </summary>
        public virtual string FirstName { get { return UserContainer.FirstName; } }

        /// <summary>
        /// The user's gender
        /// </summary>
        public virtual string Gender { get { return UserContainer.Gender; } }

        /// <summary>
        /// The amount of global credit the user currently has
        /// </summary>
        public virtual int GlobalCreditAmount { get { return UserContainer.GlobalCreditAmount; } }

        /// <summary>
        /// The user's last name
        /// </summary>
        public virtual string LastName { get { return UserContainer.LastName; } }

        /// <summary>
        /// The number of merchants the user has visited
        /// </summary>
        public virtual int MerchantsVisitedCount { get { return UserContainer.MerchantsVisitedCount; } }

        /// <summary>
        /// The number of orders the user has placed
        /// </summary>
        public virtual int OrdersCount { get { return UserContainer.OrdersCount; } }

        /// <summary>
        /// The date & time the user accepted the LevelUp terms at
        /// </summary>
        public virtual DateTime? TermsAcceptedAt
        {
            get { return Utils.TryParseDate(UserContainer.TermsAcceptedAt, Constants.Iso8601DateTimeFormat); }
        }

        /// <summary>
        /// The amount of money the user has saved since joining LevelUp
        /// </summary>
        public virtual int TotalSavingsAmount { get { return UserContainer.TotalSavingsAmount; } }

        [JsonProperty(PropertyName = "user")]
        private UserContainer UserContainer { get; set; }

        public override string ToString()
        {
            return UserContainer.ToString();
        }
    }
}
