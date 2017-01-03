#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IManageRemoteCheckData.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface IManageRemoteCheckData : ILevelUpClientModule
    {
        /// <summary>
        /// Create check data in a remote datastore
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="createRequest">An object containing details related to the data to create.</param>
        UpdateRemoteCheckDataResponse CreateRemoteCheckData(string accessToken, RemoteCheckDataRequestBody createRequest);

        /// <summary>
        /// Finalize check data in a remote datastore
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="checkUuid">The unique identifier associated with the check to finialize.</param>
        /// <param name="finalizeRequest">An object containing details related to the data on the check.</param>
        FinalizeRemoteCheckResponse FinalizeRemoteCheck(string accessToken,
                                                        string checkUuid,
                                                        FinalizeRemoteCheckRequestBody finalizeRequest);

        /// <summary>
        /// Query check data in a remote datastore
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="checkUuid">The unique identifier associated with the check to query.</param>
        GetRemoteCheckDataResponse GetRemoteCheckData(string accessToken, string checkUuid);

        /// <summary>
        /// Update check data in a remote datastore
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="checkUuid">The unique identifier associated with the check to update.</param>
        /// <param name="checkDataRequest">An object containing details related to the data to update.</param>
        UpdateRemoteCheckDataResponse UpdateRemoteCheckData(string accessToken,
                                                            string checkUuid,
                                                            RemoteCheckDataRequestBody checkDataRequest);
    }
}
