#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IModifyUser.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface IModifyUser : ILevelUpClientModule
    {
        /// <summary>
        /// Registers and returns a newly-created user
        /// </summary>
        /// <param name="apiKey">The LevelUp API key for this app</param>
        /// <param name="firstName">The new user's first name</param>
        /// <param name="lastName">The new user's last name</param>
        /// <param name="email">The new user's email</param>
        /// <param name="password">The new user's password</param>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        User CreateUser(string apiKey, string firstName, string lastName, string email, string password);

        /// <summary>
        /// Registers and returns a newly-created user
        /// </summary>
        /// <param name="apiKey">The LevelUp API key for this App</param>
        /// <param name="request">The request to create a user</param>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        User CreateUser(string apiKey, CreateUserRequestBodyUserSection request);

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="request">The request to update the user</param>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        User UpdateUser(string accessToken, UpdateUserRequestBody request);

        /// <summary>
        /// Issues a password reset request
        /// </summary>
        /// <param name="email">The users email address</param>
        void PasswordResetRequest(string email);
    }
}
