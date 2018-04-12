#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Request.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.Models.Requests
{
    public abstract class Request
    {
        protected Request(string accessToken)
        {
            AccessToken = accessToken;
        }

        /// <summary>
        /// AccessToken object containing any header token data required to 
        /// make the request.  If no token data is required, this can be null. 
        /// </summary>
        public readonly string AccessToken;

        /// <summary>
        /// Classes which derive from Request must specify and popluate a bitmask of 
        /// LevelUp platform API versions for which the particular request is valid.
        /// </summary>
        protected abstract LevelUpApiVersion _applicableAPIVersionsBitmask { get; }

        /// <summary>
        /// The latest (highest) LevelUp platform API version for which this request is valid.
        /// </summary>
        public virtual LevelUpApiVersion ApiVersion => LatestApplicableVersion;

        /// <summary>
        /// Will retrieve the highest LevelUpApiVersion that is specified in the derived
        /// object's _applicableAPIVersionsBitmask.
        /// </summary>
        private LevelUpApiVersion LatestApplicableVersion
        {
            get
            {
                int[] enumValues = (int[])Enum.GetValues(typeof(LevelUpApiVersion));
                Array.Sort(enumValues, (a, b) => b - a);    // OrderByDecending

                int versionsBitmask = (int)_applicableAPIVersionsBitmask;
                foreach (int enumValue in enumValues)
                {
                    if ((versionsBitmask & enumValue) != 0)
                    {
                        return (LevelUpApiVersion)enumValue;
                    }
                }

                throw new InvalidOperationException("The request object of type [" + this.GetType() + 
                    "] does not specify any applicable platform API verisons.");
            }
        }
    }
}

