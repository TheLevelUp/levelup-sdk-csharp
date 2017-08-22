#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CustomModelSerializerUnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Test.Models.CustomSerializationModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Test.Models
{
    [TestClass]
    public class CustomModelSerializerUnitTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_CustomSerializableModel()
        {
            var customSerializableModel = GetExampleCustomSerializableModel();
            CustomSerializableModel toSerialize = customSerializableModel.Item1;
            string expectedSerializedStr = customSerializableModel.Item2;

            var serialized = JsonConvert.SerializeObject(toSerialize);
            TestUtilities.VerifyJsonIsEquivalent<CustomSerializableModel>(serialized, expectedSerializedStr);

            var deserialized = JsonConvert.DeserializeObject<CustomSerializableModel>(serialized);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(toSerialize.PropertyA, deserialized.PropertyA);
            Assert.AreEqual(toSerialize.PropertyB, deserialized.PropertyB);
            Assert.AreEqual(toSerialize.PropertyC, deserialized.PropertyC);
            Assert.IsNull(deserialized.PropertyC);
            Assert.AreEqual(toSerialize.PropertyUnNamed, deserialized.PropertyUnNamed);
            Assert.AreEqual(deserialized.PropertyIgnored, default(int));
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_NonCustomSerializableModel()
        {
            var standardSerializableModel = GetExampleNonCustomSerializableModel();
            NonCustomSerializableModel toSerialize = standardSerializableModel.Item1;
            string expectedSerializedStr = standardSerializableModel.Item2;

            var serialized = JsonConvert.SerializeObject(toSerialize);
            TestUtilities.VerifyJsonIsEquivalent<NonCustomSerializableModel>(serialized, expectedSerializedStr);

            var deserialized = JsonConvert.DeserializeObject<CustomSerializableModel>(serialized);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(toSerialize.PropertyA, deserialized.PropertyA);
            Assert.AreEqual(toSerialize.PropertyB, deserialized.PropertyB);
            Assert.AreEqual(toSerialize.PropertyC, deserialized.PropertyC);
            Assert.IsNull(deserialized.PropertyC);
            Assert.AreEqual(toSerialize.PropertyUnNamed, deserialized.PropertyUnNamed);
            Assert.AreEqual(deserialized.PropertyIgnored, default(int));
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_ComposedSerializableModel()
        {
            var customSerializableModelDetails = GetExampleCustomSerializableModel();
            CustomSerializableModel custom = customSerializableModelDetails.Item1;
            string customExpectedSerializedStr = customSerializableModelDetails.Item2;

            var standardSerializableModelDetails = GetExampleNonCustomSerializableModel();
            NonCustomSerializableModel standard = standardSerializableModelDetails.Item1;
            string standardExpectedSerializedStr = standardSerializableModelDetails.Item2;

            var toSerialize = new ComposedSerializableModel();
            toSerialize.NormalProperty = "test_string_3";
            toSerialize.CustomSerializableSubModel = custom;
            toSerialize.NonCustomSerializableSubModel = standard;

            string serialized = JsonConvert.SerializeObject(toSerialize);

            string expectedSerializedStr = "{ \"composed_serializable_model\": { " +
                                                    "\"normal_property\": \"test_string_3\", " +
                                                    "\"custom_serializable_sub_model\":" + customExpectedSerializedStr + ", " +
                                                    "\"non_custom_serializable_sub_model\":" + standardExpectedSerializedStr +
                                            "} }";

            TestUtilities.VerifyJsonIsEquivalent<ComposedSerializableModel>(serialized, expectedSerializedStr);

            var deserialized = JsonConvert.DeserializeObject<ComposedSerializableModel>(serialized);
            Assert.IsNotNull(deserialized);
            Assert.AreEqual(toSerialize.NormalProperty, deserialized.NormalProperty);
            
            Assert.IsNotNull(deserialized.CustomSerializableSubModel);
            Assert.AreEqual(toSerialize.CustomSerializableSubModel.PropertyA, deserialized.CustomSerializableSubModel.PropertyA);
            Assert.AreEqual(toSerialize.CustomSerializableSubModel.PropertyB, deserialized.CustomSerializableSubModel.PropertyB);
            Assert.AreEqual(toSerialize.CustomSerializableSubModel.PropertyC, deserialized.CustomSerializableSubModel.PropertyC);
            Assert.AreEqual(toSerialize.CustomSerializableSubModel.PropertyUnNamed, deserialized.CustomSerializableSubModel.PropertyUnNamed);
            Assert.AreEqual(deserialized.CustomSerializableSubModel.PropertyIgnored, default(int));

            Assert.IsNotNull(deserialized.NonCustomSerializableSubModel);
            Assert.AreEqual(toSerialize.NonCustomSerializableSubModel.PropertyA, deserialized.NonCustomSerializableSubModel.PropertyA);
            Assert.AreEqual(toSerialize.NonCustomSerializableSubModel.PropertyB, deserialized.NonCustomSerializableSubModel.PropertyB);
            Assert.AreEqual(toSerialize.NonCustomSerializableSubModel.PropertyC, deserialized.NonCustomSerializableSubModel.PropertyC);
            Assert.AreEqual(toSerialize.NonCustomSerializableSubModel.PropertyUnNamed, deserialized.NonCustomSerializableSubModel.PropertyUnNamed);
            Assert.AreEqual(deserialized.NonCustomSerializableSubModel.PropertyIgnored, default(int));
        }

        private const string propA = "test_string";
        private const int propB = 6;
        private static readonly int? propC = null;
        private const string propCStr = "null";
        private const int propIgnored = 7;
        private const int propUnNamed = 8;

        private static Tuple<CustomSerializableModel, string> GetExampleCustomSerializableModel()
        {
            var toSerialize = new CustomSerializableModel();
            toSerialize.PropertyA = propA;
            toSerialize.PropertyB = propB;
            toSerialize.PropertyC = propC;
            toSerialize.PropertyIgnored = propIgnored;
            toSerialize.PropertyUnNamed = propUnNamed;

            string expectedSerializedStr = "{ " +
                                                "\"custom_serializable_model\": { " +
                                                    "\"property_a\": \"" + propA +"\", " +
                                                    "\"property_b\": " + propB + ", " +
                                                    "\"property_c\": " + propCStr + ", " +
                                                    "\"PropertyUnNamed\": " + propUnNamed +
                                                "} " +
                                            "}";

            return new Tuple<CustomSerializableModel, string>(toSerialize, expectedSerializedStr);
        }

        private static Tuple<NonCustomSerializableModel, string> GetExampleNonCustomSerializableModel()
        {
            var toSerialize = new NonCustomSerializableModel();
            toSerialize.PropertyA = propA;
            toSerialize.PropertyB = propB;
            toSerialize.PropertyC = null;
            toSerialize.PropertyIgnored = propIgnored;
            toSerialize.PropertyUnNamed = propUnNamed;

            string expectedSerializedStr = "{ " +
                                                "\"property_a\": \"" + propA + "\", " +
                                                "\"property_b\": "+ propB + ", " +
                                                "\"property_c\": " + propCStr + ", " +
                                                "\"PropertyUnNamed\": "+ propUnNamed +
                                            "}";

            return new Tuple<NonCustomSerializableModel, string>(toSerialize, expectedSerializedStr);
        }
    }
}
