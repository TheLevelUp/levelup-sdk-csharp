#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryCreditCards.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    public interface IQueryCreditCards : ILevelUpClientModule
    {
        /// <summary>
        /// Returns a list of the current user's active credit cards. 
        /// </summary>
        /// <remarks>
        /// Inactive cards include deleted cards and 
        /// duplicate cards. These records will not appear in the list.
        /// </remarks>
        /// <param name="accessToken">The LevelUp access token associated with the user account obtained from 
        /// the Authenticate() method</param>
        /// <returns>List of credit cards associated with the user's account</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        IList<CreditCard> ListCreditCards(string accessToken);
    }
}
