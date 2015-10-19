//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="OrderContainerBase.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Globalization;
using Newtonsoft.Json;
using System;

namespace LevelUp.Api.Client.Models.Responses
{
    [JsonObject]
    internal class OrderContainerBase
    {
        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmount { get; set; }

        [JsonProperty(PropertyName = "tip_amount")]
        public int TipAmount { get; set; }

        [JsonProperty(PropertyName = "total_amount")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "uuid")]
        public string Identifier { get; set; }

        public override string ToString()
        {
            return string.Format(new CultureInfo("en-US"),
                                 "Id: {0}{1}Spent: {2:C2}{1}Tip: {3:C2}{1}Total: {4:C2}{1}",
                                 Identifier,
                                 Environment.NewLine,
                                 (double) SpendAmount/100,
                                 (double) TipAmount/100,
                                 (double) Total/100);
        }
    }
}
