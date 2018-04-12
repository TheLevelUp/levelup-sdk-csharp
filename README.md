LevelUp C# SDK
===

The LevelUp C# SDK provides client-server communication with the LevelUp web service.  This SDK was designed primarily to facilitate integrating point-of-sale systems with the LevelUp ecosystem.

Table of Contents
---
  * [How To Get These Libraries](#how-to-get-these-libraries)
  * [SDK Versioning](#sdk-versioning)
  * [Getting Started](#getting-started)
  * [Configuring Your API Key](#configuring-your-api-key)
  * [Configuring the Environment](#configuring-the-environment)
    * [Sandbox Environment](#levelup-sandbox-environment)
    * [Production Environment](#levelup-production-environment)
  * [Requirements For Building The SDK](#requirements-for-building-the-sdk)
  * [TLS Requirements For The LevelUp API](#tls-requirements-for-the-levelup-api)
  * [Building the SDK](#building-the-sdk)
  * [Running the Tests](#running-the-tests)
    * [Unit Tests](#unit-tests)
    * [Integration Tests](#integration-tests)
  * [Coding Against This SDK](#coding-against-this-sdk)
    * [SDK Code Sample: Basic Authentication](#sdk-code-sample-basic-authentication)
    * [SDK Code Sample: Placing an Order](#sdk-code-sample-placing-an-order)
  * [Developer Terms](#developer-terms)
  * [Responsible Disclosure Policy](#responsible-disclosure-policy)
  * [License (Apache 2.0)](#license-apache-20)

How To Get These Libraries
---
The LevelUp C# SDK is packaged as a set of 2 dynamic libraries: **LevelUp.API.Client** and **LevelUp.API.Http**.  You may integrate these libraries into your development environment in one of two ways:
- The SDK libraries are distributed as NuGet packages via the [LevelUp MyGet Package Feed.](https://www.myget.org/gallery/levelup)  To pull these libraries via the NuGet Package Manager in Visual Studio, add one of the following to your "package sources" in the NuGet settings:
    - **NuGet V3 feed URL (Visual Studio 2015+):** https://www.myget.org/F/levelup/api/v3/index.json
    - **NuGet V2 feed URL (Visual Studio 2012+):** https://www.myget.org/F/levelup/api/v2
    - **NuGet V1 feed URL (old API for NuGet prior to v1.6 and Orchard):** https://www.myget.org/F/levelup/api/v1
- If you so desire, you may find instructions below for cloning this repo and building the LevelUp C# SDK from source.

SDK Versioning
---
The LevelUp C# SDK conforms to [Semantic Versioning](http://semver.org/).  Major version bumps indicate breaking interface changes, and are not necessarily indicative of the significance of software updates.

Releases will be provided in a rolling manner as new features are available or needed, and are not subject to a fixed release schedule.

Getting Started
---
Before you can make any progress with the LevelUp C# Integration SDK, you will need to [sign up for a LevelUp account](https://www.thelevelup.com/users/new). Once you have a LevelUp account, you can log in to your Developer Center [[Sandbox](https://www.thelevelup.com/developer/sandbox/apps) | [Production](https://www.thelevelup.com/developer/production/apps)].

Configuring Your API Key
---
In order to make calls to the LevelUp API using this SDK, you will need to obtain a LevelUp API key from your Developer Center account [[Sandbox](https://www.thelevelup.com/developer/sandbox/apps) | [Production](https://www.thelevelup.com/developer/production/apps)]. This key will be required to authenticate calls to the LevelUp platform.

We **strongly** recommend that you do not store your API key on your production system or hard-code the key in software.

Configuring the Environment
---
The user of this SDK will specify, prior to authenticating, which LevelUpEnvironment to target.  In the absence of an explicit selection, the SDK will default to the **LevelUp Sandbox Environment**.

### LevelUp Sandbox Environment
The LevelUp Sandbox Environment is designed to be a data-persistent testing environment. In this environment, any orders created will **not use real money** and customer's accounts will not be charged. This is a great environment to do your testing in!

### LevelUp Production Environment
Once you are ready to move to production, you will need to obtain another LevelUp API Key for the [Production Environment](https://www.thelevelup.com/developer/production/apps).  If you create an order in the LevelUp Production Environment, **real money will change hands** and any relevant accounts will be charged.

Requirements For Building The SDK
---
The LevelUp SDK is designed to work with .NET 4.6+ and later and was built using Visual Studio’s MSBuild system. The SDK depends on the following libraries, which are managed in the solution via [NuGet](https://www.nuget.org/):
- [RestSharp](http://restsharp.org/)
- [NewtonSoft Json.NET](http://james.newtonking.com/projects/json-net.aspx)
- [Moq](https://github.com/Moq/moq4/wiki/Quickstart)
- [FluentAssertions](http://www.fluentassertions.com/)
- [Compare-NET-Objects](https://github.com/GregFinzer/Compare-Net-Objects)

Note that Moq, FluentAssertions, and Compare-Net-Objects are only required to build the tests and are not part of the production dependency chain.

TLS Requirements For The LevelUp API
---
As part of compliance with PCI security standards, LevelUp requires that all HTTPS connections to LevelUp (```www.thelevelup.com``` and ```api.thelevelup.com```) are using TLS 1.2.  Clients using this SDK must make sure that they are configured to do so.

Beginning with .NET 4.6, TLS 1.2 is enabled as a communication protocol by default.  As a result, we suggest targeting .NET 4.6+ in client code.

For more information related to ensuring client systems are configured for TLS 1.2, please see [this article](http://blog.thelevelup.com/pci-security-is-your-restaurant-ready/).  LevelUp provides a [TLS Patcher Utility](https://github.com/TheLevelUp/pos-tls-patcher) to facilitate the process of updating.

Building The SDK
---
- [Clone this repository](https://git-scm.com/book/en/v2/Git-Basics-Getting-a-Git-Repository#Cloning-an-Existing-Repository)
- Open the "LevelUp.Sdk.sln" solution file in Visual Studio
- Right click on the solution element in the Solution Explorer
- Select "Rebuild Solution" from the drop down menu
- You may ignore the error that says `The command "xcopy /Y "%PathToSdkFolder%\LevelUp.Api.Client.Test\TestData\test_config_settings.xml" "%PathToSdkFolder%\LevelUp.Api.Client.Test\bin\Net40\"" exited with code 4.` for the moment. It will become relevant for running the unit tests.

Running the Tests
---
Several automated tests have been written to ensure the continued quality of the LevelUp C# Integration SDK. We have included the source code and projects for these tests so that you may inspect, augment, and run them if you like.

These tests are written against the MSTest framework, and are categorized (via the MSTest "TestCategory" attribute) as either **Unit Tests**, **Functional Tests** or **Integration Tests**.  Those labeled as "unit tests" or "functional tests" are designed to run near-instantaneously and without external dependencies.  Those tests which have been labeled as "integration tests" hit API endpoints in the LevelUp Sandbox environment at sandbox.thelevelup.com.

### Unit/Functional Tests
The unit/functional tests can be run without any additional configuration.  Within the Visual Studio test explorer window (Test >> Windows >> Test Explorer), group tests by "trait".  You can then select and run all tests under the "Unit Test" or "Functional Test" category.  You can also configure these tests to run automatically following project compilation (left as an exercise for the reader!)

### Integration Tests
Tests marked as "integration tests" tend to hit endpoints in our sandbox testing environment.  Therefore, in order to be able to run the tests, there are several additional steps you must take to set up and configure data related to the remote environment.

First, you must modify the file named `%PathToSdkFolder%\pos-csharp-sdk\LevelUp.Api.Client.Test\TestData\test_config_settings.xml.example` in a text editor and fill in the fields with your account information. Then save the file and rename it to "test_config_settings.xml". The fields you will have to modify are as follows: App API Key, Merchant Account Username, Merchant Account Password, Merchant Account Id, Id of a location for that merchant marked visible, & Id of a location for that merchant NOT marked as visible. You may also, optionally, update the other fields at your discretion.

Coding Against This SDK
---
This SDK wraps calls to the LevelUp platform API, which is documented on the [LevelUp Developer Site](http://developer.thelevelup.com/api-reference/). The high-level interfaces which you will use to call into these endpoints are located in `%PathToSdkFolder%\pos-csharp-sdk\LevelUp.Api.Client\ClientInterfaces\` and you can create these interfaces using the LevelUpClientFactory.  This process is illustrated in the example below.

SDK Code Sample: Basic Authentication
---

```C#
// Choose a LevelUpEnvironment. (The SDK provides built-in support for the LevelUp sandbox, staging,
// and production environments.  Additionally, you could specify a custom environment, that might target
// stubbed endpoints on your own test servers for instance, by proving a custom base uri to the
// LevelUpEnvironment.CreateEnvironment(...) method.  For details see LevelUpEnvironment class documentation.)
var environment = LevelUpEnvironment.Sandbox;

// Create a descriptive identifier (the provided data is functionally immaterial, however details
// are recorded in logs on the backend and may help to identify/diagnose issues with a request.)
var identifier = new AgentIdentifier("My company name", "My product name", "v1.6.03", "Windows .Net 4.0");

IAuthenticate authenticator = LevelUpClientFactory.Create<IAuthenticate>(identifier, environment);

// Get the token (note that this may throw a LevelUpApiException for invalid credentials.)
string myAuthToken = authenticator.Authenticate(apiKey, username, password).Token;
```

SDK Code Sample: Placing an Order
---
```C#

var exampleItems = new List<Item>( new Item[]
{
    new Item( "Astronaut's Awesome Ice Cream Sundae", "Liquid-Oxygen-cooled, dehydrated ice cream topped with all the best junk " +
    "food on the planet that you're currently orbiting!", "LEV-JQ-PL-36-GG-C50", "043200005264", "Desserts", 100, 100)
});

const decimal TAX_RATE = 0.05m;
int itemTotal = exampleItems.Aggregate(0, (total, item) => total + item.ChargedPriceCents);
int tax = Decimal.ToInt32(itemTotal * TAX_RATE);    // Your integration will likely do something much smarter here.

IManageProposedOrders orderManager = LevelUpClientFactory.Create<IManageProposedOrders>(identifier, environment);

// Propose an order to register your intent to place an order with the LevelUp platform and to find out if there is
// any available merchant-funded credit. Note that this may throw a LevelUpApiException for invalid credentials.
var proposedOrder = orderManager.CreateProposedOrder(   accessToken: myToken,
                                                        locationId: 345678,                                                    // [1]
                                                        qrPaymentData: "LU321...456LU",
                                                        spendAmountCents: itemTotal + tax,
                                                        taxAmountCents: tax,
                                                        exemptionAmountCents: 0,                                               // [2]
                                                        register: "Moe's Bar and Grill Register 3",
                                                        cashier: "Jane",
                                                        identifierFromMerchant: "3456782",                                     // [3]
                                                        receiptMessageHtml:  "Thanks for eating at <strong>Moe's</strong>",    // [4]
                                                        items: exampleItems);


// Recalculate the tax after applying any available discount credit.
var newItemTotal = itemTotal - proposedOrder.DiscountAmountCents;
var newTaxAmount = Decimal.ToInt32(newItemTotal * TAX_RATE);

var completedOrder = orderManager.CompleteProposedOrder(accessToken: myToken,
                                                        locationId: 345678,
                                                        qrPaymentData: "LU321...456LU",
                                                        proposedOrderUuid: proposedOrder.ProposedOrderIdentifier,
                                                        spendAmountCents: newItemTotal + newTaxAmount,
                                                        taxAmountCents: newTaxAmount,
                                                        exemptionAmountCents: 0,
                                                        appliedDiscountAmountCents: proposedOrder.DiscountAmountCents,
                                                        register: "Moe's Bar and Grill Register 3",
                                                        cashier: "Jane",
                                                        identifierFromMerchant: "3456782",
                                                        receiptMessageHtml:  "Thanks for eating at <strong>Moe's</strong>",
                                                        items: exampleItems );

```
**Notes from above example:**

[1] The LevelUp platform assigns each merchant store a unique location Id.  This value (or values if the merchant has multiple sites) will be listed in your merchant dashboard.

[2] Total amount for items that are exempt from earning loyalty or having loyalty discount credit applied (ex. alcohol, in some states.)

[3] A unique identifier for the order which can be identified at the POS.   This is typically the order ID or number for the check.

[4] Gets added to the LevelUp email receipt.



Developer Terms
---
By enabling LevelUp integrations, including through this SDK, you agree to LevelUp's [developer terms](https://www.thelevelup.com/developer-terms).

Responsible Disclosure Policy
---
LevelUp takes the security of its users and the safety of their information very seriously.  If you’ve discovered a vulnerability, help us fix it by [disclosing your findings to us](https://www.thelevelup.com/security-response).

License (Apache 2.0)
---
```
Copyright 2016 SCVNGR, Inc. d/b/a LevelUp

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```

**Note:** This solution uses the [License Header Manager](https://visualstudiogallery.msdn.microsoft.com/5647a099-77c9-4a49-91c3-94001828e99e) extension for Visual Studio to manage and update the license text at the top of each source file, instructions for which are documented [here](https://github.com/rubicon-oss/LicenseHeaderManager/wiki).
