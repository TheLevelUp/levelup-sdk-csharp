#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Address.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models
{
    /// <summary>
    /// Class representing an address object for a business or individual
    /// </summary>
    [JsonObject]
    public class Address
    {
        private Address()
        {
            // Private constructor for deserialization
        }

        /// <summary>
        /// Constructor for the Address object
        /// </summary>
        /// <param name="name">Name of the addressee</param>
        /// <param name="streetAddress">Line 1 of street address of addressee. e.g. 123 Main St.</param>
        /// <param name="extendedAddress">Line 2 of street address of addressee. e.g. Apt. #1A</param>
        /// <param name="city">City or town of addressee</param>
        /// <param name="region">Region, county, or province of addressee</param>
        /// <param name="postalCode">Zip code or postal code for addressee</param>
        /// <param name="country">Nation of addressee</param>
        /// <param name="coordinates">Latitude and longitudinal coordinates of addressee for mapping</param>
        /// <param name="website">Web site for addressee</param>
        public Address(string name, 
                       string streetAddress = "", 
                       string extendedAddress = "", 
                       string city = "", 
                       string region = "", 
                       string postalCode = "", 
                       string country = "U.S.A.", 
                       GeographicLocation coordinates = null)
        {
            Name = name;
            StreetAddress = streetAddress;
            ExtendedAddress = extendedAddress;
            City = city;
            Region = region;
            PostalCode = postalCode;
            Country = country;
            GeographicCoordinates = coordinates;
        }

        /// <summary>
        /// Name of the addressee
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Basic street address. e.g. 123 Main st.
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Address line 2. Apartment number etc.
        /// </summary>
        public string ExtendedAddress { get; set; }

        /// <summary>
        /// The city of the addressee
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The state/province of the address
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Zip code for addressee
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Nation where addressee lives
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Geographic coordinates of the addressee if appropriate. May be set to null.
        /// </summary>
        public GeographicLocation GeographicCoordinates { get; set; }
    }
}
