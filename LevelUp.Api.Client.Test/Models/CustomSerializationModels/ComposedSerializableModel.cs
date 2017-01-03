#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ComposedSerializableModel.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models;
using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Test.Models.CustomSerializationModels
{
    /// <summary>
    /// Example class that specifies the custom LevelUpModelSerializer for 
    /// json serialization/deserialization, and contains properties
    /// that are both "ModelSerializable" and non-"ModelSerializable".
    /// Essentially, this model has sub-models, only some of which use
    /// the cusom serializer.
    /// </summary>
    [JsonObject]
    [LevelUpSerializableModel("composed_serializable_model")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class ComposedSerializableModel : IResponse
    {
        [JsonProperty(PropertyName = "normal_property")]
        public string NormalProperty { get; internal set; }

        [JsonProperty(PropertyName = "custom_serializable_sub_model")]
        public CustomSerializableModel CustomSerializableSubModel { get; internal set; }

        [JsonProperty(PropertyName = "non_custom_serializable_sub_model")]
        public NonCustomSerializableModel NonCustomSerializableSubModel { get; internal set; }
    }
}
