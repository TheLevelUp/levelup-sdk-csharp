#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateDetachedRefundIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Http;
using NUnit.Framework;

namespace LevelUp.Api.Client.Tests.Client.IntegrationTests
{
    [TestFixture, Explicit]
    public class ICreateDetachedRefundIntegrationTests
    {
        [Test]
        public void CreateDetachedRefund()
        {
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();

            const int refundAmountCents = 50;
            ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer(refundAmountCents);

            ICreateDetachedRefund refundInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateDetachedRefund>();
            var refundData = new DetachedRefundRequestBody( LevelUpTestConfiguration.Current.MerchantLocationId, 
                                                            LevelUpTestConfiguration.Current.ConsumerQrData, 
                                                            refundAmountCents);
            var refund = refundInterface.CreateDetachedRefund(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, refundData);
            Assert.AreEqual(refund.CreditAmountCents, refundAmountCents);
        }

        [Test]
        public void CreateFails_WhenQrCodeIsInvalid()
        {
            const string invalidQRCode = "LU02000ROETEST_BAD_QR_DATA_D40200A0LU";

            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();

            const int refundAmountCents = 50;
            ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer(refundAmountCents);

            ICreateDetachedRefund refundInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateDetachedRefund>();
            var refundData = new DetachedRefundRequestBody( LevelUpTestConfiguration.Current.MerchantLocationId, 
                                                            invalidQRCode, 
                                                            refundAmountCents);

            try
            {
                refundInterface.CreateDetachedRefund(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, refundData);
                Assert.Fail("Expected LevelUpApiException on refund with bad Qr data but did not catch it!");
            }
            catch (LevelUpApiException) { }

            // Cleanup, refund the order.
            refundData = new DetachedRefundRequestBody( LevelUpTestConfiguration.Current.MerchantLocationId, 
                                                        LevelUpTestConfiguration.Current.ConsumerQrData, 
                                                        refundAmountCents);
            refundInterface.CreateDetachedRefund(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, refundData);
        }
    }
}
