#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IAuthenticate.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    public interface IAuthenticate : ILevelUpClientModule
    {
        /// <summary>
        /// Obtain a LevelUp access token.  This token is required to make most calls to the LevelUp API.
        /// </summary>
        /// <param name="apiKey">
        /// Your LevelUp API key.  This key will be sent to you by LevelUp when you sign up for a developer 
        /// account.  Alternatively, your API key should be available when you log into the Developer Center.
        /// </param>
        /// <param name="username">Your LevelUp username</param>
        /// <param name="password">Your LevelUp password</param>
        /// <returns>
        /// A LevelUp access token object.  That object has a "Token" field, which stores the access token 
        /// string you will need to provide to subsequent LevelUp requests.
        /// </returns>
        /// <exception cref="LevelUpApiException">
        ///  The returned HTTP status code for the request was something other than 200 (OK)
        /// </exception>
        AccessToken Authenticate(string apiKey, string username, string password);
    }
}
