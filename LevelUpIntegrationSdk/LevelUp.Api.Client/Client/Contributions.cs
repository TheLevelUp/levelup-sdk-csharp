//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Contributions.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2015 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace LevelUp.Api.Client
{
    public partial class LevelUpClient
    {
        /// <summary>
        /// Gets the details of the specified contribution target
        /// </summary>
        /// <param name="contributionTargetId">Identifies the contribution target for which to return the details</param>
        /// <returns>Detailed info about the contibution target</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public ContributionTarget GetContributionTarget(string contributionTargetId)
        {
            string uri = _endpoints.ContributionTargetDetails(contributionTargetId);

            IRestResponse response = _restService.Get(uri, null, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<ContributionTarget>(response.Content);
        }

        /// <summary>
        /// List of all contribution targets to which users may donate their LevelUp savings.
        /// </summary>
        /// <returns>A list of all the contribution targets.</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public IList<ContributionTarget> ListContributionTargets()
        {
            string uri = _endpoints.ContributionTargets();

            IRestResponse response = _restService.Get(uri, null, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<List<ContributionTarget>>(response.Content);
        }
    }
}
