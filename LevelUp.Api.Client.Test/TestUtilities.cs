#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="TestUtilities.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Test
{
    public class TestUtilities
    {
        public static bool PublicPropertiesAreEqual<T>(T a, T b) where T : class
        {
            CompareLogic logic = new CompareLogic();
            ComparisonResult result = logic.Compare(a, b);

            return result.AreEqual;
        }

        public static T SerializeThenDeserialize<T>(T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        public static void VerifyJsonIsEquivalent<T>(string objAsJson1, string objAsJson2) where T : class
        {
            T obj1 = JsonConvert.DeserializeObject<T>(objAsJson1) as T;
            T obj2 = JsonConvert.DeserializeObject<T>(objAsJson2) as T;

            obj1.Should().NotBeNull();
            obj2.Should().NotBeNull();

            obj1.ShouldBeEquivalentTo(obj2);
        }
    }
}
