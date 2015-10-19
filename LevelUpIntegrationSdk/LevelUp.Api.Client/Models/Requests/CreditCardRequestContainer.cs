//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreditCardRequestContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class CreditCardRequestContainer
    {
        public CreditCardRequestContainer(string encryptedNumber,
                                   string encryptedExpirationMonth,
                                   string encryptedExpirationYear,
                                   string encryptedCvv,
                                   string postalCode)
        {
            EncryptedNumber = encryptedNumber;
            EncryptedExpirationMonth = encryptedExpirationMonth;
            EncryptedExpirationYear = encryptedExpirationYear;
            EncryptedCvv = encryptedCvv;
            PostalCode = postalCode;
        }

        [JsonProperty(PropertyName = "encrypted_cvv")]
        public string EncryptedCvv { get; set; }

        [JsonProperty(PropertyName = "encrypted_expiration_month")]
        public string EncryptedExpirationMonth { get; set; }

        [JsonProperty(PropertyName = "encrypted_expiration_year")]
        public string EncryptedExpirationYear { get; set; }

        [JsonProperty(PropertyName = "encrypted_number")]
        public string EncryptedNumber { get; set; }

        [JsonProperty(PropertyName = "postal_code")]
        public string PostalCode { get; set; }
    }
}
