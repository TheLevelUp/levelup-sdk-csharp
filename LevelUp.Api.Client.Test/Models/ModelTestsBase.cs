#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ModelTestsBase.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Test.Models
{
    public class ModelTestsBase
    {
        private const string JSON_NULL_STRING = "null";

        protected string FormatJsonString(string jsonString)
        {
            return null == jsonString ? JSON_NULL_STRING : "\"" + jsonString + "\"";
        }

        protected string FormatJsonNullableType(int? jsonNullable)
        {
            return jsonNullable.HasValue ? jsonNullable.Value.ToString() : JSON_NULL_STRING;
        }
    }
}
