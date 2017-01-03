#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ContributionTarget.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Globalization;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp contribution target
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("contribution_target")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class ContributionTarget : IResponse
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private ContributionTarget() { }

        /// <summary>
        /// Internal constructor for testing
        /// </summary>
        internal ContributionTarget(string description, bool? employerRequired, string facebookUrl, string id, 
            string name, string partnerSpecificTerms, string twitterUsername, string website, int? minimumAgeRequired, 
            bool? homeAddressRequired, bool? featured)
        {
            Description = description;
            EmployerRequired = employerRequired;
            FacebookUrl = facebookUrl;
            Id = id;
            Name = name;
            PartnerSpecificTerms = partnerSpecificTerms;
            TwitterUsername = twitterUsername;
            Website = website;
            MinimumAgeRequiredInternal = minimumAgeRequired;
            HomeAddressRequiredInternal = homeAddressRequired;
            FeaturedInternal = featured;
        }

        /// <summary>
        /// An HTML description of the target
        /// </summary>
        [JsonProperty(PropertyName = "description_html")]
        public string Description { get; private set; }

        /// <summary>
        /// Whether the user must specify his or her employer to donate to the target
        /// </summary>
        [JsonProperty(PropertyName = "employer_required")]
        public bool? EmployerRequired { get; private set; }

        /// <summary>
        /// The URL of the target's Facebook page
        /// </summary>
        [JsonProperty(PropertyName = "facebook_url")]
        public string FacebookUrl { get; private set; }

        /// <summary>
        /// The target's Id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// The name of the target
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Terms which must be presented to the user before he or she agrees to donate to the target
        /// </summary>
        [JsonProperty(PropertyName = "partner_specific_terms")]
        public string PartnerSpecificTerms { get; private set; }

        /// <summary>
        /// The target's Twitter username, without the leading "@"
        /// </summary>
        [JsonProperty(PropertyName = "twitter_username")]
        public string TwitterUsername { get; private set; }

        /// <summary>
        /// The target's website
        /// </summary>
        [JsonProperty(PropertyName = "website")]
        public string Website { get; private set; }

        /// <summary>
        /// The minimum age required to donate to the target
        /// </summary>
        [JsonIgnore]
        public int MinimumAgeRequired { get { return MinimumAgeRequiredInternal.GetValueOrDefault(0); } }

        [JsonProperty(PropertyName = "minimum_age_required")]
        private int? MinimumAgeRequiredInternal { get; set; }

        /// <summary>
        /// Whether the user must specify his or her home address to donate to the target
        /// </summary>
        [JsonIgnore]
        public bool HomeAddressRequired { get { return HomeAddressRequiredInternal == true; } }

        [JsonProperty(PropertyName = "home_address_required")]
        private bool? HomeAddressRequiredInternal { get; set; }

        /// <summary>
        /// Whether the target is being actively promoted
        /// </summary>
        [JsonIgnore]
        public bool Featured { get { return FeaturedInternal == true; } }

        [JsonProperty(PropertyName = "FeaturedInternal")]
        private bool? FeaturedInternal { get; set; }

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
