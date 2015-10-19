//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="UpdateUserRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2014 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Class representing a request to update a user.  Only values that you want to update need to be specified
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class UpdateUserRequest
    {
        public UpdateUserRequest(int id)
        {
            Id = id;
            UserContainer = new UpdateUserRequestContainer();
        }

        /// <summary>
        /// The user's birthday
        /// </summary>
        public DateTime? BornAt
        {
            get
            {
                return Utils.TryParseDate(UserContainer.BornAt.GetValueOrDefault(), Constants.Iso8601DateFormat);
            }
            set
            {
                UserContainer.BornAt = value.HasValue ? value.Value.ToString(Constants.Iso8601DateFormat) : null;
            }
        }

        /// <summary>
        /// Arbitrary key-value data to associate with this user
        /// </summary>
        public Dictionary<string, string> CustomAttributes
        {
            get { return UserContainer.CustomAttributes.GetValueOrDefault(); }
            set { UserContainer.CustomAttributes = value; }
        }

        /// <summary>
        /// The user's email address
        /// </summary>
        public string Email
        {
            get { return UserContainer.Email.GetValueOrDefault(); }
            set { UserContainer.Email = value; }
        }

        /// <summary>
        /// The user's first name
        /// </summary>
        public string FirstName
        {
            get { return UserContainer.FirstName.GetValueOrDefault(); }
            set { UserContainer.FirstName = value; }
        }

        /// <summary>
        /// The user's gender
        /// </summary>
        public string Gender
        {
            get { return UserContainer.Gender.GetValueOrDefault(); }
            set { UserContainer.Gender = value; }
        }

        /// <summary>
        /// The id of the user
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        public string LastName
        {
            get { return UserContainer.LastName.GetValueOrDefault(); }
            set { UserContainer.LastName = value; }
        }

        /// <summary>
        /// The user's new password
        /// </summary>
        public string NewPassword
        {
            get { return UserContainer.NewPassword.GetValueOrDefault(); }
            set { UserContainer.NewPassword = value; }
        }

        [JsonProperty(PropertyName = "user")]
        private UpdateUserRequestContainer UserContainer { get; set; }
    }
}
