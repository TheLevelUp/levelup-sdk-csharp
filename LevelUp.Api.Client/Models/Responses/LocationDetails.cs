#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LocationDetails.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp merchant location details
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("location")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class LocationDetails : IResponse
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private LocationDetails() { }

        public LocationDetails(int locationId, string menuUrl, int merchantId, string merchantName, bool isVisible, 
            List<int> categories, string extendedAddress, string facebookUrl, string hours, double latitude, 
            double longitude, string locality, string merchantDescription, string postalCode, string region, 
            string streetAddress, string timeOfLastUpdate, string name, string tipPreference)
        {
            LocationId = locationId;
            MenuUrl = menuUrl;
            MerchantId = merchantId;
            MerchantName = merchantName;
            IsVisible = isVisible;
            Categories = categories;
            ExtendedAddress = extendedAddress;
            FacebookUrl = facebookUrl;
            Hours = hours;
            Latitude = latitude;
            Longitude = longitude;
            Locality = locality;
            MerchantDescription = merchantDescription;
            PostalCode = postalCode;
            Region = region;
            StreetAddress = streetAddress;
            TimeOfLastUpdate = timeOfLastUpdate;
            Name = name;
            TipPreference = tipPreference;
        }

        /// <summary>
        /// Address of the location
        /// </summary>
        public virtual Address Address
        {
            get
            {
                return new Address(MerchantName,
                                   StreetAddress,
                                   ExtendedAddress,
                                   Locality,
                                   Region,
                                   PostalCode,
                                   "U.S.A",
                                   new GeographicLocation(Latitude, Longitude));
            }
        }

        /// <summary>
        /// Latitude and Longitude coordinates of the location for mapping
        /// </summary>
        public virtual GeographicLocation LocationCoordinates
        {
            get { return new GeographicLocation(Latitude, Longitude); }
        }

        /// <summary>
        /// The ID of the location
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int LocationId { get; private set; }

        /// <summary>
        /// Url for the locations' menu
        /// </summary>
        [JsonIgnore]
        [JsonProperty(PropertyName = "menu_url")]
        public string MenuUrl { get; private set; }

        /// <summary>
        /// ID of the merchant whose location this is
        /// </summary>
        [JsonProperty(PropertyName = "merchant_id")]
        public int MerchantId { get; private set; }

        /// <summary>
        /// Name of the merchant whose location this is
        /// </summary>
        [JsonProperty(PropertyName = "merchant_name")]
        public string MerchantName { get; private set; }

        /// <summary>
        /// Returns true if the location is set to be visible to users.
        /// </summary>
        [JsonProperty(PropertyName = "shown")]
        public bool IsVisible { get; private set; }

        
        [JsonProperty(PropertyName = "categories")]
        private List<int> Categories { get; set; }

        [JsonProperty(PropertyName = "extended_address")]
        private string ExtendedAddress { get; set; }

        [JsonProperty(PropertyName = "facebook_url")]
        private string FacebookUrl { get; set; }

        [JsonProperty(PropertyName = "hours")]
        private string Hours { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        private double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        private double Longitude { get; set; }
        
        [JsonProperty(PropertyName = "locality")]
        private string Locality { get; set; }
        
        [JsonProperty(PropertyName = "merchant_description_html")]
        private string MerchantDescription { get; set; }

        [JsonProperty(PropertyName = "postal_code")]
        private string PostalCode { get; set; }

        [JsonProperty(PropertyName = "region")]
        private string Region { get; set; }

        [JsonProperty(PropertyName = "street_address")]
        private string StreetAddress { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        private string TimeOfLastUpdate { get; set; }

        [JsonProperty(PropertyName = "name")]
        private string Name { get; set; }

        [JsonProperty(PropertyName = "merchant_tip_preference")]
        private string TipPreference { get; set; }

        public override string ToString()
        {
            if (!IsVisible)
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
