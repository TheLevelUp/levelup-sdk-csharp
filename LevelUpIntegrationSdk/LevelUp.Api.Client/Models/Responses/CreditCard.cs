//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreditCard.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2015 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CreditCard
    {
        public CreditCard()
        {
            CreditCardContainer = new CreditCardContainer();
        }

        /// <summary>
        /// Whether or not the credit card is active
        /// </summary>
        public bool Active { get { return "active".Equals(State, StringComparison.InvariantCultureIgnoreCase); } }

        /// <summary>
        /// The first six digits of the card number
        /// </summary>
        public string Bin { get { return CreditCardContainer.Bin; } }

        /// <summary>
        /// A human-readable description of the credit card
        /// </summary>
        public string Description { get { return CreditCardContainer.Description; } }

        /// <summary>
        /// The month in which the card expires
        /// </summary>
        public int ExpirationMonth { get { return CreditCardContainer.ExpirationMonth; } }

        /// <summary>
        /// The year in which the card expires
        /// </summary>
        public int ExpirationYear { get { return CreditCardContainer.ExpirationYear; } }

        /// <summary>
        /// The credit card's ID
        /// </summary>
        public int Id { get { return CreditCardContainer.Id; } }

        /// <summary>
        /// The last four characters of the card number
        /// </summary>
        public string Last4Numbers { get { return CreditCardContainer.Last4Numbers; } }

        /// <summary>
        /// Whether the card is the user's promoted payment instrument
        /// </summary>
        public bool Promoted { get { return CreditCardContainer.Promoted; } }

        /// <summary>
        /// The card's state. Possible values: "active" - the card is available to be charged, 
        /// "inactive" - the card may not be charged
        /// </summary>
        public string State { get { return CreditCardContainer.State; } }

        /// <summary>
        /// The type of card, e.g. Visa, Mastercard
        /// </summary>
        public string Type { get { return CreditCardContainer.Type; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "credit_card")]
        private CreditCardContainer CreditCardContainer { get; set; }

        public override string ToString()
        {
            return CreditCardContainer.ToString();
        }
    }
}
