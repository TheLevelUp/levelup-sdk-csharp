using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a managed LevelUp merchant location.  Note that this location schema
    /// is different from the one in a standard merchant location query.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("location")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class ManagedLocation : IResponse
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private ManagedLocation() { }

        /// <summary>
        /// Internal constructor for testing
        /// </summary>
        internal ManagedLocation(int locationId, int merchantId, string name, string merchantName, 
            string address, string tipPreference)
        {
            LocationId = locationId;
            Name = name;
            TipPreference = tipPreference;
        }

        /// <summary>
        /// Identification number for the location
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int LocationId { get; private set; }

        /// <summary>
        /// Identification number for the merchant
        /// </summary>
        [JsonProperty(PropertyName = "merchant_id")]
        public int MerchantId { get; private set; }

        /// <summary>
        /// Address of the location
        /// </summary>
        [JsonProperty(PropertyName = "address")]
        public string Address { get; private set; }

        /// <summary>
        /// Name of the location
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        /// <summary>
        /// Name of the merchant
        /// </summary>
        [JsonProperty(PropertyName = "merchant_name")]
        public string MerchantName { get; private set; }

        /// <summary>
        /// Tip preference for the location
        /// </summary>
        [JsonProperty(PropertyName = "tip_preference")]
        public string TipPreference { get; private set; }

        public override string ToString()
        {
            return string.Format("Name: {1}{0}Location Id: {2}{0} Merchant Name: {3}{0} " +
                                 "Merchant ID: {4}{0}, Address: {5}{0} Tip Preference: {6}{0}",
                                 Environment.NewLine, Name, LocationId, MerchantName, MerchantId, 
                                 Address, TipPreference);
        }
    }
}
