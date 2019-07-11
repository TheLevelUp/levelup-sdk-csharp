﻿#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryUser.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface IQueryUser : ILevelUpClientModule
    {
        /// <summary>
        /// Returns a list of a user's street addresses.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the user account obtained from 
        /// the Authenticate() method</param>
        /// <returns>A list of the user's addresses</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        IList<UserAddress> ListUserAddresses(string accessToken);

        /// <summary>
        /// Returns details about a user account. Users, including merchant users, may only retrieve their own 
        /// user details.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the user account obtained from 
        /// the Authenticate() method</param>
        /// <param name="userId">The LevelUp ID for the user to query</param>
        /// <returns>Detailed info about the user</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        User GetUser(string accessToken, int userId);

        /// <summary>
        /// Returns the total amount of credit that a user is eligible to redeem at a particular location.
        /// </summary>
        /// <param name="userAccessToken">The LevelUp access token associated with the user account obtained from 
        /// the Authenticate() method</param>
        /// <param name="locationId">The LevelUp ID for the location to query</param>
        /// <returns>Total credit available to a user at selected location</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        Credit GetLocationUserCredit(string userAccessToken, int locationId);
    }
}
