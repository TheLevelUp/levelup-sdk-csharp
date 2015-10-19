//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Users.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace LevelUp.Api.Client
{
    public partial class LevelUpClient
    {
        /// <summary>
        /// Registers and returns a newly-created user
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="request">The request to create a user</param>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public User CreateUser(string accessToken, CreateUserRequest request)
        {
            string uri = _endpoints.Users();

            string body = JsonConvert.SerializeObject(request);

            IRestResponse response = _restService.Post(uri, body, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<User>(response.Content);
        }

        /// <summary>
        /// Returns details about a user account. Normal users, including merchants, may only retrieve their own 
        /// user details. Admins may retrieve any user
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="userId">Identifies the user to get</param>
        /// <returns>Detailed info about the user</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public User GetUser(string accessToken, int userId)
        {
            string uri = _endpoints.User(userId);

            IRestResponse response = _restService.Get(uri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<User>(response.Content);
        }

        /// <summary>
        /// Returns a list of a user's associated street addresses.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <returns>A list of the user's addresses</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public IList<UserAddress> ListUserAddresses(string accessToken)
        {
            string uri = _endpoints.UserAddresses();

            IRestResponse response = _restService.Get(uri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<List<UserAddress>>(response.Content);
        }

        /// <summary>
        /// Issues a password reset request
        /// </summary>
        /// <param name="email">The users email address</param>
        public void PasswordResetRequest(string email)
        {
            string uri = _endpoints.Passwords();

            string body = JsonConvert.SerializeObject(new {email});

            IRestResponse response = _restService.Post(uri, body, null, Identifier.ToString());

            // when successful a 204 no-content will be returned
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                ThrowExceptionOnBadResponseCode(response);
            } 
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="request">The request to update the user</param>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public User UpdateUser(string accessToken, UpdateUserRequest request)
        {
            string uri = _endpoints.User(request.Id);

            string body = JsonConvert.SerializeObject(request);

            IRestResponse response = _restService.Put(uri, body, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<User>(response.Content);
        }
    }
}
