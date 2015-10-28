//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreditCardTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;
using LevelUp.Api.Client.Models.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test
{
    [TestClass]
    [DeploymentItem("LevelUpBaseUri.txt")]
    [DeploymentItem("test_config_settings.xml")]
    public class CreditCardTests : ApiUnitTestsBase
    {
        [TestMethod]
        public void ListCreditCards()
        {
            IList<CreditCard> creditCards = Api.ListCreditCards(AccessToken.Token);
            Assert.IsNotNull(creditCards);
            Assert.IsTrue(creditCards.Count > 0);
            Assert.IsNotNull(creditCards[0]);
        }

        [TestMethod]
        public void PromoteCreditCard()
        {
            IList<CreditCard> creditCards = Api.ListCreditCards(AccessToken.Token);
            Assert.IsTrue(creditCards.Count > 0);

            CreditCard result = Api.PromoteCreditCard(AccessToken.Token, creditCards[0].Id);
            Assert.IsNotNull(result);
        }
    }
}
