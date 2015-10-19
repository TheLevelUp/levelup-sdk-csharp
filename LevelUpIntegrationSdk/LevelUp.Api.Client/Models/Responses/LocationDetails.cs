//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LocationDetails.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp merchant location details
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class LocationDetails
    {
        public LocationDetails()
        {
            LocationContainer = new LocationContainer();
        }

        /// <summary>
        /// Address of the location
        /// </summary>
        public virtual Address Address
        {
            get
            {
                return new Address(LocationContainer.MerchantName,
                                   LocationContainer.StreetAddress,
                                   LocationContainer.ExtendedAddress,
                                   LocationContainer.Locality,
                                   LocationContainer.Region,
                                   LocationContainer.PostalCode,
                                   "U.S.A",
                                   new GeographicLocation(LocationContainer.Latitude, LocationContainer.Longitude));
            }
        }

        /// <summary>
        /// The ID of the location
        /// </summary>
        public virtual int LocationId { get { return LocationContainer.LocationId; } }

        /// <summary>
        /// Url for the locations' menu
        /// </summary>
        [JsonIgnore]
        public virtual string MenuUrl { get { return LocationContainer.MenuUrl; } }

        /// <summary>
        /// ID of the merchant whose location this is
        /// </summary>
        public virtual int MerchantId { get { return LocationContainer.MerchantId; } }

        /// <summary>
        /// Name of the merchant whose location this is
        /// </summary>
        public virtual string MerchantName { get { return LocationContainer.MerchantName; } }

        /// <summary>
        /// Returns true if the location is set to be visible to users. False otherwise
        /// </summary>
        public virtual bool IsVisible { get { return LocationContainer.Visible; } }

        /// <summary>
        /// Latitude and Longitude coordinates of the location for mapping
        /// </summary>
        public virtual GeographicLocation LocationCoordinates
        {
            get { return new GeographicLocation(LocationContainer.Latitude, LocationContainer.Longitude); }
        }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "location")]
        private LocationContainer LocationContainer { get; set; }

        public override string ToString()
        {
            return LocationContainer.ToString();
        }
    }
}
