#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreateUserRequestBodyUserSection.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// The request to create a new user has a subsection called "user" that contains a subset of the data 
    /// needed for the body of the request.  This object represents that subsection.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel(null)]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class CreateUserRequestBodyUserSection
    {
        private CreateUserRequestBodyUserSection()
        {
            // Private constructor for deserialization
        }

        /// <summary>
        /// Instantiates a CreateUserRequest object
        /// </summary>
        /// <param name="firstName">The new user's first name</param>
        /// <param name="lastName">The new user's last name</param>
        /// <param name="email">The new user's email</param>
        /// <param name="password">The new user's password</param>
        public CreateUserRequestBodyUserSection(string firstName, string lastName, string email, string password)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; private set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; private set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; private set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; private set; }
    }
}
