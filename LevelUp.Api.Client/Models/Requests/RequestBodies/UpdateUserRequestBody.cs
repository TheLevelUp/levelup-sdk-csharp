#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="UpdateUserRequestBody.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Globalization;
using LevelUp.Api.Utilities;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Class representing a request to update a user.  Only values that you want to update need to be specified
    /// </summary>
    /// <remarks>
    /// Implementation note: If null values are provided in the body of an update-user request for name/email/password/etc.,
    /// the request will be rejected with a 422.  As a result, each of the properties below are marked with a 
    /// [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] attribute.
    /// </remarks>
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("user")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class UpdateUserRequestBody
    {
        public UpdateUserRequestBody(int id)
        {
            Id = id;
        }

        [JsonIgnore]
        public DateTime? BornAt
        {
            get
            {
                DateTime parseDate;
                return DateTime.TryParseExact(BornAtStr, Constants.Iso8601DateFormat, Constants.EnUsCulture,
                    DateTimeStyles.AssumeLocal, out parseDate) ? parseDate : new DateTime?();
            }
            set
            {
                BornAtStr = value.HasValue ? value.Value.ToString(Constants.Iso8601DateFormat) : null;
            }
        }

        [JsonIgnore]
        public int Id { get; private set; }

        [JsonProperty(PropertyName = "born_at", NullValueHandling = NullValueHandling.Ignore)]
        public string BornAtStr { get; private set; }

        [JsonIgnore]
        public Dictionary<string, string> CustomAttributes
        {
            get { return (CustomAttributesInternal != null) ? new Dictionary<string, string>(CustomAttributesInternal) : null; }
        }

        [JsonProperty(PropertyName = "custom_attributes", NullValueHandling = NullValueHandling.Ignore)]
        private Dictionary<string, string> CustomAttributesInternal { get; set; }

        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; private set; }

        [JsonProperty(PropertyName = "first_name", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; private set; }

        [JsonProperty(PropertyName = "gender", NullValueHandling = NullValueHandling.Ignore)]
        public string Gender { get; private set; }

        [JsonProperty(PropertyName = "last_name", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; private set; }

        [JsonProperty(PropertyName = "new_password", NullValueHandling = NullValueHandling.Ignore)]
        public string NewPassword { get; private set; }
    }
}
