//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LocationContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System;
using System.Collections.Generic;

namespace LevelUp.Api.Client.Models.Responses
{
    [JsonObject]
    internal class LocationContainer : LocationContainerBase
    {
        [JsonProperty(PropertyName = "categories")]
        public List<int> Categories { get; set; }

        [JsonProperty(PropertyName = "extended_address")]
        public string ExtendedAddress { get; set; }

        [JsonProperty(PropertyName = "facebook_url")]
        public string FacebookUrl { get; set; }

        [JsonProperty(PropertyName = "hours")]
        public string Hours { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "merchant_id")]
        public int MerchantId { get; set; }

        [JsonProperty(PropertyName = "merchant_name")]
        public string MerchantName { get; set; }

        [JsonProperty(PropertyName = "locality")]
        public string Locality { get; set; }

        [JsonProperty(PropertyName = "menu_url")]
        public string MenuUrl { get; set; }

        [JsonProperty(PropertyName = "merchant_description_html")]
        public string MerchantDescription { get; set; }

        [JsonProperty(PropertyName = "postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; }

        [JsonProperty(PropertyName = "street_address")]
        public string StreetAddress { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public string TimeOfLastUpdate { get; set; }

        [JsonProperty(PropertyName = "shown")]
        public bool Visible { get; set; }

        public override string ToString()
        {
            if (!Visible)
            {
                return string.Format("Location with id {0} is not visible{1}", LocationId, Environment.NewLine);
            }

            return string.Format("Location Id: {0}{1}{2}{1}",
                                 LocationId,
                                 Environment.NewLine,
                                 new Address(MerchantName,
                                             streetAddress: StreetAddress,
                                             extendedAddress: ExtendedAddress,
                                             city: Locality,
                                             region: Region,
                                             postalCode: PostalCode,
                                             country: "U.S.A.",
                                             coordinates: new GeographicLocation(Latitude, Longitude)));
        }
    }
}
