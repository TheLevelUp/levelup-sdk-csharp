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
        private const string CONFIG_FILENAME = "test_config_settings.xml";
        private const string CONFIG_EXAMPLE_FILENAME = CONFIG_FILENAME + ".example";

        [XmlIgnore]
        private static LevelUpTestConfiguration _current = null;

        [XmlIgnore]
        public static LevelUpTestConfiguration Current
        {
            get { return _current ?? (_current = ReadTestConfiguration()); }
        }

        public string App_ApiKey { get; set; }
        public string Merchant_Username { get; set; }
        public string Merchant_Password { get; set; }
        public int Merchant_Id { get; set; }
        public int Merchant_LocationId_Visible { get; set; }
        public int Merchant_LocationId_Invisible { get; set; }
        public string User_EmailAddress { get; set; }
        public string User_FirstName { get; set; }
        public string User_LastInitial { get; set; }
        public int User_Id { get; set; }
        public string User_PaymentToken { get; set; }
        public string User_PaymentTokenWith10PercentTip { get; set; }
        public string User_InvalidPaymentToken { get; set; }
        public string User_GiftCardPaymentToken { get; set; }

        private static LevelUpTestConfiguration ReadTestConfiguration()
        {
            LevelUpTestConfiguration configuration = null;

            XmlSerializer ser = new XmlSerializer(typeof(LevelUpTestConfiguration));
            
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

            using (StreamReader reader = new StreamReader(config_filepath))
            {
                configuration = (LevelUpTestConfiguration)ser.Deserialize(reader);
            }

            return configuration;
        }
    }
}
