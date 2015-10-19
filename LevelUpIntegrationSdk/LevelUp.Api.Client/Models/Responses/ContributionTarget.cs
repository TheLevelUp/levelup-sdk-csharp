//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ContributionTarget.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2014 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp contribution target
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ContributionTarget
    {
        public ContributionTarget()
        {
            ContributionTargetContainer = new ContributionTargetContainer();
        }

        /// <summary>
        /// An HTML description of the target
        /// </summary>
        public virtual string Description { get { return ContributionTargetContainer.Description; } }

        /// <summary>
        /// Whether the user must specify his or her employer to donate to the target
        /// </summary>
        public virtual bool EmployerRequired { get { return ContributionTargetContainer.EmployerRequired == true; } }

        /// <summary>
        /// The URL of the target's Facebook page
        /// </summary>
        public virtual string FacebookUrl { get { return ContributionTargetContainer.FacebookUrl; } }

        /// <summary>
        /// Whether the target is being actively promoted
        /// </summary>
        public virtual bool Featured { get { return ContributionTargetContainer.Featured == true; } }

        /// <summary>
        /// Whether the user must specify his or her home address to donate to the target
        /// </summary>
        public virtual bool HomeAddressRequired
        {
            get { return ContributionTargetContainer.HomeAddressRequired == true; }
        }

        /// <summary>
        /// The target's Id
        /// </summary>
        public virtual string Id { get { return ContributionTargetContainer.Id; } }

        /// <summary>
        /// The minimum age required to donate to the target
        /// </summary>
        public virtual int MinimumAgeRequired
        {
            get { return ContributionTargetContainer.MinimumAgeRequired.GetValueOrDefault(0); }
        }

        /// <summary>
        /// The name of the target
        /// </summary>
        public virtual string Name { get { return ContributionTargetContainer.Name; } }

        /// <summary>
        /// Terms which must be presented to the user before he or she agrees to donate to the target
        /// </summary>
        public virtual string PartnerSpecificTerms
        {
            get { return ContributionTargetContainer.PartnerSpecificTerms; }
        }

        /// <summary>
        /// The target's Twitter username, without the leading "@"
        /// </summary>
        public virtual string TwitterUsername { get { return ContributionTargetContainer.TwitterUsername; } }

        /// <summary>
        /// The target's website
        /// </summary>
        public virtual string Website { get { return ContributionTargetContainer.Website; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "contribution_target")]
        private ContributionTargetContainer ContributionTargetContainer { get; set; }

        public override string ToString()
        {
            return ContributionTargetContainer.ToString();
        }
    }
}
