//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Address.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Text;

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
            //Private constructor for deserialization
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
            this.Name = name;
            this.StreetAddress = streetAddress;
            this.ExtendedAddress = extendedAddress;
            this.City = city;
            this.Region = region;
            this.PostalCode = postalCode;
            this.Country = country;
            this.GeographicCoordinates = coordinates;
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

        [Obsolete("Use Region Property Instead")]
        public string State { get; set; }

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

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool includeValueDescriptors)
        {
            StringBuilder sb = new StringBuilder();

            AppendLineIfNotNullOrEmpty(ref sb, Name, "Name", includeValueDescriptors);
            AppendLineIfNotNullOrEmpty(ref sb, StreetAddress, "Address 1", includeValueDescriptors);
            AppendLineIfNotNullOrEmpty(ref sb, ExtendedAddress, "Address 2", includeValueDescriptors);

            AppendLineIfNotNullOrEmpty(ref sb,
                                       string.Format("{0}, {1} {2}",
                                                     City,
                                                     Region,
                                                     PostalCode),
                                       "Locale",
                                       includeValueDescriptors);

            AppendLineIfNotNullOrEmpty(ref sb, Country, "Country", includeValueDescriptors);

            return sb.ToString();
        }

        private void AppendLineIfNotNullOrEmpty(ref StringBuilder builder,
                                                string value,
                                                string valueDescriptor,
                                                bool includeValueDescriptor)
        {
            if (!string.IsNullOrEmpty(value))
            {
                builder.AppendLine(includeValueDescriptor
                                       ? string.Format("{0}: {1}", valueDescriptor, value)
                                       : value);
            }
        }
    }
}
