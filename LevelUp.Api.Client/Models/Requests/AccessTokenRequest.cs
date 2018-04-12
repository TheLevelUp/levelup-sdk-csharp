#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="AccessTokenRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Http;

namespace LevelUp.Api.Client.Models.Requests
{
    public class AccessTokenRequest : Request
    {
        protected override LevelUpApiVersion _applicableAPIVersionsBitmask
        {
            get { return LevelUpApiVersion.v14 | LevelUpApiVersion.v15; }
        }

        /// <summary>
        /// A Serializable http body for the AccessTokenRequest
        /// </summary>
        public AccessTokenRequestBody Body { get; }

        /// <summary>
        /// Constructor for a LevelUp access token request
        /// </summary>
        /// <param name="apiKey">Your LevelUp API key</param>
        /// <param name="username">The merchant's LevelUp user name</param>
        /// <param name="password">The merchant's LevelUp password</param>
        public AccessTokenRequest(string apiKey, string username, string password)
            : base(null)
        {
            Body = new AccessTokenRequestBody(apiKey, username, password);
        }
    }
}
