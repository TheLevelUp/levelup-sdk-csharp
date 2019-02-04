#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpTestConfiguration.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace LevelUp.Api.Client.Tests
{
    /// <summary>
    /// In order to run the LevelUp integration tests, it is required that you provide a 
    /// configuration file containing data that pertains to your test users, etc.  These
    /// integration tests assume that you have a particular setup in the sandbox environment, 
    /// specifically one standard user (Consumer) with a linked credit card, one standard
    /// user (ConsumerWithNoLinkedPayment) that has no payment method linked, and one 
    /// merchant user (Merchant) who is set up to manage a merchant site with at least one location. 
    /// </summary>
    [XmlRoot(ElementName = "LevelUpTestConfiguration")]
    public class LevelUpTestConfiguration
    {
        #region Singleton Creation
        
        [XmlIgnore]
        private static LevelUpTestConfiguration _current = null;

        [XmlIgnore]
        public static LevelUpTestConfiguration Current
        {
            get { return _current ?? (_current = ReadTestConfiguration()); }
        }

        #endregion

        #region Configuration Properties

        public string MerchantUsername { get; set; }
        public string MerchantPassword { get; set; }
        public string MerchantEmailAddress { get; set; }

        public string ConsumerUsername { get; set; }
        public string ConsumerPassword { get; set; }
        public string ConsumerEmailAddress { get; set; }
        public string ConsumerUserFirstName { get; set; }
        public string ConsumerUserLastInitial { get; set; }

        public string ConsumerWithNoLinkedPaymentUsername { get; set; }
        public string ConsumerWithNoLinkedPaymentPassword { get; set; }
        public string ConsumerWithNoLinkedPaymentEmailAddress { get; set; }

        /// <summary>
        /// Your app's API key, which will be used to generate an authentication token.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The ID of the merchant associated with your merchant user.
        /// </summary>
        public int MerchantId { get; set; }

        /// <summary>
        /// The Id of the location associated with your test merchant.
        /// </summary>
        public int MerchantLocationId { get; set; }

        /// <summary>
        /// The QR code associated with your Consumer user
        /// </summary>
        public string ConsumerQrData { get; set; }

        /// <summary>
        /// The QR code associated with your Consumer user, with a 10% tip added. Note that the tip is specified via
        /// the last two letters in the QR code data before the trailing "LU".
        /// </summary>
        public string ConsumerQrDataWith10PercentTip { get; set; }

        /// <summary>
        /// The QR code associated with your second consumer user that has no payment method linked to their account.
        /// </summary>
        public string ConsumerWithNoLinkedPaymentQrData { get; set; }

        /// <summary>
        /// The QR code associated with a gift card linked to the Consumer user's account.
        /// </summary>
        public string GiftCardData { get; set; }

        /// <summary>
        /// The encrypted string for a test credit card number on sandbox.  See this list of test credit cards provided by LevelUp:
        /// http://developer.thelevelup.com/api-reference/getting-started/test-credit-cards/
        /// Note that this value should be the braintree-encrypted form of one of those credit cards.
        /// </summary>
        public string TestCreditCardEncryptedNumber { get; set; }

        /// <summary>
        /// The encrypted string for a test credit card Cvv code on sandbox.  See this list of test credit cards provided by LevelUp:
        /// http://developer.thelevelup.com/api-reference/getting-started/test-credit-cards/
        /// Note that this value should be the braintree-encrypted form of one of those credit cards cvv codes.
        /// </summary>
        public string TestCreditCardEncryptedCvv { get; set; }

        /// <summary>
        /// The name of your company.  Unchecked, but this information helps LevelUp determine who is making requests to our API 
        /// for diagnostic purposes.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// The name of your product.  Unchecked, but this information helps LevelUp determine who is making requests to our API 
        /// for diagnostic purposes.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// The version of your product. Unchecked, but this information helps LevelUp determine who is making requests to our API 
        /// for diagnostic purposes.
        /// </summary>
        public string ProductVersion { get; set; }

        /// <summary>
        /// The operating system on which your application is running.  Unchecked, but this information helps LevelUp determine who 
        /// is making requests to our API for diagnostic purposes.
        /// </summary>
        public string OperatingSystem { get; set; }


        /// <summary>
        /// The id of the test consumer user.
        /// </summary>
        public int ConsumerId { get; set; }

        #endregion

        #region Read in the configuration file

        private static LevelUpTestConfiguration ReadTestConfiguration()
        {
            XmlSerializer ser = new XmlSerializer(typeof(LevelUpTestConfiguration));

            string configFilepath = GetConfigurationFilePath();
            VerifyConfigurationFileExists(configFilepath);

            using (StreamReader reader = new StreamReader(configFilepath))
            {
                return (LevelUpTestConfiguration)ser.Deserialize(reader);
            }
        }

        private static void VerifyConfigurationFileExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                return;
            }

            const string exampleFileName = "test_config_settings.xml.example";

            string errorMessage = String.Format(
                "Configuration file '{0}' not found.  This file is required to run LevelUp tests.{1}" +
                "Please copy and rename the example file '{2}' (located in the same directory as the" +
                "executing assembly) and enter your LevelUp data in the appropriate fields in order " +
                "to run LevelUp tests.",
                filePath, Environment.NewLine, exampleFileName);
            throw new FileNotFoundException(errorMessage);
        }

        private static string GetConfigurationFilePath()
        {
            string configFilename = Environment.GetEnvironmentVariable("LevelUpTestConfigurationFileName");
            if (string.IsNullOrEmpty(configFilename))
            {
                configFilename = "test_config_settings.xml";
            }

            string configFilePath = Environment.GetEnvironmentVariable("LevelUpTestConfigurationFilePath");
            if (string.IsNullOrEmpty(configFilePath))
            {
                configFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }

            return Path.Combine(configFilePath, configFilename);
        }

        #endregion
    }
}
