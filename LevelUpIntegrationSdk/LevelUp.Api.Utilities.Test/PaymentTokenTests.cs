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
            [DeploymentItem("qr_codes.csv")]
            [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
                "qr_codes.csv",
                "qr_codes#csv",
                DataAccessMethod.Sequential)]
            public void Redact_DataDriven()
            {
                string qrCode = (string) context.DataRow["QrCode"];
                string expectedValue = (string) context.DataRow["Redacted"];

                PaymentToken.QrCode.Redact(qrCode).ShouldBeEquivalentTo(expectedValue);
            }
        }
    }
}
