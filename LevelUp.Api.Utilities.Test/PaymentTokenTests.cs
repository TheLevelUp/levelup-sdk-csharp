#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="PaymentTokenTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Utilities.Test
{
    [TestClass]
    public class PaymentTokenTests
    {
        [TestClass]
        public class QrCodeTests
        {
            private TestContext context;

            public TestContext TestContext
            {
                get { return context; }
                set { context = value; }
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            [DeploymentItem("qr_codes.csv")]
            [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                "qr_codes.csv",
                "qr_codes#csv", 
                DataAccessMethod.Sequential)]
            public void IsProbablyValid_DataDriven()
            {
                string qrCode = (string) context.DataRow["QrCode"];
                bool expectedValue = bool.Parse((string)context.DataRow["Valid"]);

                PaymentToken.QrCode.IsProbablyValid(qrCode).Should().Be(expectedValue);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            [DeploymentItem("qr_codes.csv")]
            [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                "qr_codes.csv",
                "qr_codes#csv",
                DataAccessMethod.Sequential)]
            public void Redact_DataDriven()
            {
                string qrCode = (string) context.DataRow["QrCode"];
                string expectedValue = (string) context.DataRow["Redacted"];

                string redacted = PaymentToken.QrCode.Redact(qrCode);
                redacted.Should().Be(expectedValue);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            [DeploymentItem("qr_codes.csv")]
            [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                "qr_codes.csv",
                "qr_codes#csv",
                DataAccessMethod.Sequential)]
            public void StripTip_DataDriven()
            {
                string qrCode = (string) context.DataRow["QrCode"];
                string expectedValue = (string) context.DataRow["TipStripped"];

                PaymentToken.QrCode.StripTip(qrCode).Should().Be(expectedValue);
            }
        }
    }
}
