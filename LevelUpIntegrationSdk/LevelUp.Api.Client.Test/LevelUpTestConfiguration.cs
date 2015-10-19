//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpTestConfiguration.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2015 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace LevelUp.Api.Client.Test
{
    [XmlRoot(ElementName = "LevelUpTestConfiguration")]
    public class LevelUpTestConfiguration
    {
        [XmlIgnore]
        private static LevelUpTestConfiguration _current = null;

        [XmlIgnore]
        public static LevelUpTestConfiguration Current
        {
            get { return _current ?? (_current = ReadTestConfiguration()); }
        }

        public string ClientId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }

        public string QrData { get; set; }
        public string QrDataWith10PercentTip { get; set; }

        public string InvalidQrData { get; set; }

        public string GiftCardData { get; set; }

        private static LevelUpTestConfiguration ReadTestConfiguration()
        {
            XmlSerializer ser = new XmlSerializer(typeof(LevelUpTestConfiguration));
            
            const string CONFIG_FILENAME = "test_config_settings.xml";
            const string CONFIG_EXAMPLE_FILENAME = "test_config_settings.xml.example";
            string config_files_directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string config_filepath = Path.Combine(config_files_directory, CONFIG_FILENAME);

            if (!File.Exists(config_filepath))
            {
                StringBuilder errorMessageBuilder = new StringBuilder();
                errorMessageBuilder.AppendLine(String.Format(
                    "Configuration file '{0}' not found.  This file is required to run LevelUp tests.",
                        config_filepath));

                errorMessageBuilder.AppendLine(String.Format(
                    "Please copy and rename the example file '{0}' (located in the same directory) and enter your " +
                    "LevelUp data in the appropriate fields in order to run LevelUp tests.", CONFIG_EXAMPLE_FILENAME));

                errorMessageBuilder.AppendLine("in the appropriate fields in order to run LevelUp tests.");

                throw new FileNotFoundException(errorMessageBuilder.ToString());
            }

            LevelUpTestConfiguration configuration = null;

            using (StreamReader reader = new StreamReader(config_filepath))
            {
                configuration = (LevelUpTestConfiguration)ser.Deserialize(reader);
            }

            return configuration;
        }
    }
}
