//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreditCardRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Class representing a LevelUp credit card request object
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CreditCardRequest
    {
        private CreditCardRequest()
        {
            //Private constructor for deserialization
        }

        /// <summary>
        /// Request to add a new credit to a user's account.  The credit card information must be encrypted with
        /// LevelUp's Braintree public key
        /// </summary>
        /// <param name="encryptedNumber">The card number, encrypted with LevelUp's Braintree public key</param>
        /// <param name="encryptedExpirationMonth">The month in which the card expires, encrypted with LevelUp's Braintree 
        /// public key</param>
        /// <param name="encryptedExpirationYear">The year in which the card expires, encrypted with LevelUp's Braintree 
        /// public key</param>
        /// <param name="encryptedCvv">The card verification value, encrypted with LevelUp's Braintree public key</param>
        /// <param name="postalCode">The postal code portion of the card's billing address (not encrypted)</param>
        public CreditCardRequest(string encryptedNumber,
                                 string encryptedExpirationMonth,
                                 string encryptedExpirationYear,
                                 string encryptedCvv,
                                 string postalCode)
        {
            CreditCardRequestContainer = new CreditCardRequestContainer(encryptedNumber,
                                                                        encryptedExpirationMonth, 
                                                                        encryptedExpirationYear, 
                                                                        encryptedCvv, 
                                                                        postalCode);
        }

        /// <summary>
        /// The card verification value, encrypted with LevelUp's Braintree public key
        /// </summary>
        public string EncryptedCvv { get { return CreditCardRequestContainer.EncryptedCvv; } }

        /// <summary>
        /// The month in which the card expires, encrypted with LevelUp's Braintree public key
        /// </summary>
        public string EncryptedExpirationMonth { get { return CreditCardRequestContainer.EncryptedExpirationMonth; } }

        /// <summary>
        /// The year in which the card expires, encrypted with LevelUp's Braintree public key
        /// </summary>
        public string EncryptedExpirationYear { get { return CreditCardRequestContainer.EncryptedExpirationYear; } }

        /// <summary>
        /// The card number, encrypted with LevelUp's Braintree public key
        /// </summary>
        public string EncryptedNumber { get { return CreditCardRequestContainer.EncryptedNumber; } }

        /// <summary>
        /// The postal code portion of the card's billing address (not encrypted)
        /// </summary>
        public string PostalCode { get { return CreditCardRequestContainer.PostalCode; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "credit_card")]
        private CreditCardRequestContainer CreditCardRequestContainer { get; set; }
    }
}
