#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Location.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a basic LevelUp merchant location
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("location")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class Location : IResponse
    {
        /// <summary>
        /// Constructor for deserialization
        /// </summary>
        // TODO: Make this private on next major version
        public Location() { }

        public Location(int locationId, string name, string tipPreference)
        {
            LocationId = locationId;
            Name = name;
            TipPreference = tipPreference;
        }

        /// <summary>
        /// Identification number for the location
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int LocationId { get; private set; }

        /// <summary>
        /// Name of the location
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        /// <summary>
        /// Tip preference for the location
        /// </summary>
        [JsonProperty(PropertyName = "merchant_tip_preference")]
        public string TipPreference { get; private set; }

        public override string ToString()
        {
            return string.Format("Name: {0}{1}Location Id: {2}{1}",
                                 Name,
                                 Environment.NewLine,
                                 LocationId);
        }
    }
}
