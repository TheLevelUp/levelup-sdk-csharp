#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ILookupUserLoyalty.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface ILookupUserLoyalty : ILevelUpClientModule
    {
        /// <summary>
        /// Gets details about a loyalty - i.e. the relationship between a user and a merchant. 
        /// If a user has no existing loyalty with the merchant, an "empty" loyalty is returned
        /// </summary>
        /// <param name="userAccessToken">The LevelUp access token associated with the user's account</param>
        /// <param name="merchantId">The ID of the LevelUp merchant to get the loyalty info for</param>
        /// <returns>Detailed info about the loyalty</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        Loyalty GetLoyalty(string userAccessToken, int merchantId);
    }
}
