#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateGiftCardValue.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface ICreateMerchantFundedCredit : ILevelUpClientModule
    {
        /// <summary>
        /// Grants a merchant funded credit through LevelUp
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="email">The email address of the recipient of the credit</param>
        /// <param name="durationInSeconds">Time before credit expires (in seconds)</param>
        /// <param name="merchantId">ID of merchant funding the credit</param>
        /// <param name="message">Message indicating why or how the user earned the reward</param>
        /// <param name="valueAmount">Total amount of the credit (in cents)</param>
        /// <param name="global">Allow credit to be used at any merchant. Default is false</param>
        void GrantMerchantFundedCredit(
            string accessToken,
            string email,
            int durationInSeconds,
            int merchantId,
            string message,
            int valueAmount,
            bool global);
    }
}
