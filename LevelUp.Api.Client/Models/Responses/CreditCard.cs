#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreditCard.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using JsonEnvelopeSerializer;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("credit_card")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class CreditCard : Response
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        public CreditCard() { }

        public CreditCard(string bin, string description, int expirationMonth, int expirationYear, int id,
            string last4Numbers, bool promoted, string state, string type)
        {
            Bin = bin;
            Description = description;
            ExpirationMonth = expirationMonth;
            ExpirationYear = expirationYear;
            Id = id;
            Last4Numbers = last4Numbers;
            Promoted = promoted;
            State = state;
            Type = type;
        }

        /// <summary>
        /// Whether or not the credit card is active
        /// </summary>
        public bool Active { get { return "active".Equals(State, StringComparison.InvariantCultureIgnoreCase); } }

        /// <summary>
        /// The first six digits of the card number
        /// </summary>
        [JsonProperty(PropertyName = "bin")]
        public string Bin { get; private set; }

        /// <summary>
        /// A human-readable description of the credit card
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; private set; }

        /// <summary>
        /// The month in which the card expires
        /// </summary>
        [JsonProperty(PropertyName = "expiration_month")]
        public int ExpirationMonth { get; private set; }

        /// <summary>
        /// The year in which the card expires
        /// </summary>
        [JsonProperty(PropertyName = "expiration_year")]
        public int ExpirationYear { get; private set; }

        /// <summary>
        /// The credit card's ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; private set; }

        /// <summary>
        /// The last four characters of the card number
        /// </summary>
        [JsonProperty(PropertyName = "last_4")]
        public string Last4Numbers { get; private set; }

        /// <summary>
        /// Whether the card is the user's promoted payment instrument
        /// </summary>
        [JsonProperty(PropertyName = "promoted")]
        public bool Promoted { get; private set; }

        /// <summary>
        /// The card's state. Possible values: "active" - the card is available to be charged, 
        /// "inactive" - the card may not be charged
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public string State { get; private set; }

        /// <summary>
        /// The type of card, e.g. Visa, Mastercard
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }
    }
}
