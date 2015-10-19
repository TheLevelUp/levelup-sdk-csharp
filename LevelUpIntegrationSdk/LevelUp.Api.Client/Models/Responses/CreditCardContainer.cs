//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreditCardContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    internal class CreditCardContainer
    {
        [JsonProperty(PropertyName = "bin")]
        public string Bin { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "expiration_month")]
        public int ExpirationMonth { get; set; }

        [JsonProperty(PropertyName = "expiration_year")]
        public int ExpirationYear { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "last_4")]
        public string Last4Numbers { get; set; }

        [JsonProperty(PropertyName = "promoted")]
        public bool Promoted { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {1}{0}" +
                                 "Bin: {2}{0}" +
                                 "Description: {3}{0}" +
                                 "Expiration Month: {4}{0}" + 
                                 "Expiration Year: {5}{0}" +
                                 "Last 4 Numbers: {6}{0}" +
                                 "Promoted: {7}{0}" +
                                 "State: {8}{0}" +
                                 "Type: {9}{0}",
                                 Environment.NewLine,   
                                 Id,
                                 Bin,
                                 Description,
                                 ExpirationMonth,
                                 ExpirationYear,
                                 Last4Numbers,
                                 Promoted ? "Yes" : "No",
                                 State,
                                 Type);
        }
    }
}
