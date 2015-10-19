//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ContributionTargetContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Globalization;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ContributionTargetContainer
    {
        [JsonProperty(PropertyName = "description_html")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "employer_required")]
        public bool? EmployerRequired { get; set; }

        [JsonProperty(PropertyName = "facebook_url")]
        public string FacebookUrl { get; set; }

        [JsonProperty(PropertyName = "featured")]
        public bool? Featured { get; set; }

        [JsonProperty(PropertyName = "home_address_required")]
        public bool? HomeAddressRequired { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "minimum_age_required")]
        public int? MinimumAgeRequired { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "partner_specific_terms")]
        public string PartnerSpecificTerms { get; set; }

        [JsonProperty(PropertyName = "twitter_username")]
        public string TwitterUsername { get; set; }

        [JsonProperty(PropertyName = "website")]
        public string Website { get; set; }

        public override string ToString()
        {
            return string.Format(new CultureInfo("en-US"),
                                 "Id: {0}{1}" +
                                 "Description: {2}{1}" +
                                 "EmployerRequired: {3}{1}" +
                                 "Facebook Url: {4}{1}" +
                                 "Featured: {5}{1}" + 
                                 "HomeAddressRequired: {6}{1}" + 
                                 "MinimumAgeRequired: {7}{1}" + 
                                 "Name: {8}{1}" + 
                                 "PartnerSpecificTerms: {9}{1}" + 
                                 "TwitterUsername: {10}{1}" + 
                                 "Website: {11}{1}",
                                 Id,
                                 Environment.NewLine,
                                 Description,
                                 EmployerRequired == true ? "Y" : "N",
                                 FacebookUrl,
                                 Featured == true ? "Y" : "N",
                                 HomeAddressRequired == true ? "Y" : "N",
                                 MinimumAgeRequired,
                                 Name,
                                 PartnerSpecificTerms,
                                 TwitterUsername,
                                 Website);
        }
    }
}
