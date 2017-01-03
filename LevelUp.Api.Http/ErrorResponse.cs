#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ErrorResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using Newtonsoft.Json;

namespace LevelUp.Api.Http
{
    /// <summary>
    /// An object representing an error returned from LevelUp
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ErrorResponse
    {
        public ErrorResponse()
        {
            ErrorContainer = new ErrorContainer();
        }

        /// <summary>
        /// LevelUp object that is invalid as a result of the error
        /// </summary>
        public virtual string Object { get { return ErrorContainer.Object; } }

        /// <summary>
        /// Specific attribute or property that is in error
        /// </summary>
        public virtual string Property { get { return ErrorContainer.Property; } }

        /// <summary>
        /// Friendly error message returned from LevelUp
        /// </summary>
        public virtual string Message { get { return ErrorContainer.Message; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        private ErrorContainer ErrorContainer { get; set; }

        public override string ToString()
        {
            return ErrorContainer.ToString();
        }
    }
}
