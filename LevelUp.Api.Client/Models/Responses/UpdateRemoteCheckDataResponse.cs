#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="UpdateRemoteCheckDataResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Models.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("check")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class UpdateRemoteCheckDataResponse : IResponse
    {
        private const char LINE_SEPARATOR = ',';

        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private UpdateRemoteCheckDataResponse() { }

        /// <summary>
        /// Internal constructor for testing
        /// </summary>
        internal UpdateRemoteCheckDataResponse(string checkLocator, string checkIdentifier, string qrCodeAsUnicodeCharacters)
        {
            CheckLocator = checkLocator;
            CheckIdentifier = checkIdentifier;
            QrCodeAsUnicodeCharacters = qrCodeAsUnicodeCharacters;
        }

        /// <summary>
        /// Converts the string of unicode character codes representing an text qr code to a array of lines
        /// </summary>
        /// <param name="qrCodeUnicodeCharacters">A string of decimal digit characters representing a QR code
        /// Lines should be separated with a comma and individual characters should be separated with a space</param>
        private static string[] ParseQrCode(string qrCodeUnicodeCharacters)
        {
            if (string.IsNullOrEmpty(qrCodeUnicodeCharacters))
            {
                return null;
            }

            return qrCodeUnicodeCharacters.Split(LINE_SEPARATOR);
        }

        [JsonIgnore]
        private string[] _checkIdQrCode = null;

        [JsonIgnore]
        public string[] QrCodeSymbols
        {
            get
            {
                return _checkIdQrCode ?? (_checkIdQrCode = ParseQrCode(QrCodeAsUnicodeCharacters)); 
            }
        }

        [JsonProperty(PropertyName = "locator")]
        public string CheckLocator { get; private set; }

        /// <summary>
        /// A unique identifier for the check in the LevelUp system
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public string CheckIdentifier { get; private set; }

        /// <summary>
        /// Contains a collection of unicode character codes.
        /// lines are separated by commas (','). 
        /// The characters in use are Unicode characters
        ///  U+2584 [Lower Half Block] ▄ (maps to ASCII 220)
        ///  U+2580 [Upper Half Block] ▀ (maps to ASCII 223)
        ///  U+2588 [Full Block] █       (maps to ASCII 219)
        ///  U+00A0 [No-Break Space]     (maps to ASCII 255)
        /// </summary>
        [JsonProperty(PropertyName = "qr_code_character_art")]
        public string QrCodeAsUnicodeCharacters { get; private set; }

        public override string ToString()
        {
            return string.Format("Record locator: {0}, Uuid from LevelUp: {1}", CheckLocator, CheckIdentifier);
        }
    }
}
