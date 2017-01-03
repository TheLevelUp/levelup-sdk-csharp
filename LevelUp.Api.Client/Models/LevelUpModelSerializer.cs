#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpModelSerializer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LevelUp.Api.Client.Models
{
    /// <summary>
    /// A custom JSON Serializer/Deserializer that wraps objects with a root tag.
    /// </summary>
    /// <remarks> 
    /// This custom Json.Net (de)serializer is designed to handle objects which, when in their
    /// JSON string form, must be wrapped by a root node with the object name as a tag.  This
    /// pattern required frequently by platform endpoints.
    /// 
    /// For example:
    /// Our Access Token model object may be defined as follows:
    /// 
    /// public class AccessToken : IResponse
    /// {
    ///    public string token { get; set; }
    ///    public int user_id { get; set; }
    ///    public int merchant_id { get; set; }
    /// }
    /// 
    /// The default Json.Net behavior is to serialize this object into:
    ///     { token: "blah", user_id: "123:, merchant_id: "456" }
    /// 
    /// but what the platform endpoint requires is:
    ///     { access_token: { token: "blah", user_id: "123:, merchant_id: "456" } }
    /// 
    /// By attaching two attributes to the AccessToken class:
    ///     [LevelUpSerializableModel("access_token")]
    ///     [JsonConverter(typeof(LevelUpModelSerializer))]
    /// Json.Net (for example when you use JsonConvert.Serialize(...)), will 
    /// serialize and deserialize our AccessToken object into this latter format.
    /// </remarks>
    public class LevelUpModelSerializer : JsonConverter
    {
        #region Serialize

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject inner = GenerateJObjectTreeFromObjectProperties(value);

            LevelUpSerializableModel attribute = GetSerializableModelAttribute(value.GetType());

            if (null == attribute || string.IsNullOrEmpty(attribute.RootTag))
            {
                inner.WriteTo(writer);
            }
            else
            {
                // Insert a wrapping container JObject since the object's class specifies the LevelUpSerializableModel attribute
                JObject outer = new JObject();
                outer.AddFirst(new JProperty(attribute.RootTag, inner));
                outer.WriteTo(writer);
            }
        }

        private JObject GenerateJObjectTreeFromObjectProperties(object value)
        {
            var valueType = value.GetType();

            var jsonObjectAttribute = Attribute.GetCustomAttribute(valueType, typeof (JsonObjectAttribute)) as JsonObjectAttribute;
            bool optIn = (null != jsonObjectAttribute) && (jsonObjectAttribute.MemberSerialization == MemberSerialization.OptIn);

            JObject retval = new JObject();

            foreach (PropertyInfo prop in valueType.GetProperties(BindingFlags.NonPublic |
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                if (!prop.CanRead)
                {
                    continue;
                }

                object propertyValue = prop.GetValue(value, null);

                // Handle property-level [JsonIgnore] attribute
                JsonIgnoreAttribute ignore = Attribute.GetCustomAttribute(prop, typeof(JsonIgnoreAttribute)) as JsonIgnoreAttribute;
                if (null != ignore)
                {
                    continue;
                }

                // Handle class-level [JsonObject(MemberSerialization.OptIn)] attribute 
                JsonPropertyAttribute attr = Attribute.GetCustomAttribute(prop, typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;
                if (null == attr)
                {
                    if (!optIn)
                    {
                        retval.Add(prop.Name, (propertyValue != null) ? JToken.FromObject(propertyValue) : null);
                    }
                    continue;
                }

                string tagName = attr.PropertyName ?? prop.Name;

                if (null != propertyValue)
                {
                    retval.Add(tagName, JToken.FromObject(propertyValue));
                    continue;
                }

                // Handle property-level [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] attribute when value is null
                if (attr.NullValueHandling == NullValueHandling.Ignore)
                {
                    continue;
                }

                // Handle property-level [JsonProperty(Required = Required.Always)] attribute when value is null
                if (attr.Required == Required.Always)
                {
                    throw new JsonSerializationException(string.Format("Value for field {0} cannot be null", tagName));
                }

                retval.Add(tagName, null);
            }
            return retval;
        }

        #endregion

        #region Deserialize

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            reader.DateParseHandling = DateParseHandling.None;
            JObject jsonObject = JObject.Load(reader);

            // Strip the container JObject if the object's class specifies the LevelUpSerializableModel attribute
            LevelUpSerializableModel attribute = GetSerializableModelAttribute(objectType);
            if (null != attribute && !string.IsNullOrEmpty(attribute.RootTag))
            {
                JProperty prop = jsonObject.Property(attribute.RootTag);
                if (null != prop && prop.Value is JObject)
                {
                    jsonObject = prop.Value as JObject;
                }
            }

            return GenerateObjectFromJObjectWithReflection(jsonObject, objectType);
        }

        private object GenerateObjectFromJObjectWithReflection(JObject jsonObject, Type objectType)
        {
            object retval = Activator.CreateInstance(objectType, true);

            foreach (var retvalProperty in retval.GetType().GetProperties(BindingFlags.NonPublic |
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                // Handle any custom property names attached to the object via [JsonProperty(PropertyName="blah")]
                object[] jsonAttributes = retvalProperty.GetCustomAttributes(typeof (JsonPropertyAttribute), false);

                string tag = retvalProperty.Name;
                if (jsonAttributes.Length > 0 && jsonAttributes[0] != null)
                {
                    tag = (jsonAttributes[0] as JsonPropertyAttribute).PropertyName ?? retvalProperty.Name;
                }

                var jsonSubObject = jsonObject[tag];
                if (retvalProperty.GetSetMethod(true) != null && jsonSubObject != null)
                {
                    var toWrite = jsonSubObject.ToObject(retvalProperty.PropertyType);
                    retvalProperty.SetValue(retval, toWrite, null);
                }
            }

            return retval;
        }

        #endregion

        public override bool CanConvert(Type objectType)
        {
            return (null != GetSerializableModelAttribute(objectType));
        }

        private static LevelUpSerializableModel GetSerializableModelAttribute(Type t)
        {
            return Attribute.GetCustomAttribute(t, typeof(LevelUpSerializableModel)) as LevelUpSerializableModel;
        }
    }
}
