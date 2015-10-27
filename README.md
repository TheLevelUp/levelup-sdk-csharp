LevelUp integration SDK C#
---

This repo contains releases for the LevelUp C# Integration SDK.

Binaries are available in the [releases](https://github.com/TheLevelUp/levelup-integration-sdk-csharp/releases) section.

### Requirements
The LevelUp SDK is designed to work with the .NET 3.0 platform and was built using Visual Studio’s MSBuild system. The SDK depends on the [RestSharp](http://restsharp.org/) and [NewtonSoft Json.NET](http://james.newtonking.com/projects/json-net.aspx) libraries, which are added to the project via [NuGet](https://www.nuget.org/).

The LevelUp SDK Example App depends on the SDK library. Instructions on setting up and building the Example App source code are provided in the installation section.

### Getting Started
Before you can make any progress with the LevelUp C# Integration SDK, you will need to [sign up for a LevelUp account](http://developer.thelevelup.com/getting-started/creating-your-app/). Once you have a LevelUp account, you can log in to your [Developer Center](https://www.thelevelup.com/developer/sandbox/apps) and obtain an API Key.

### Configuring Your API Key
To use the LevelUp SDK, you will need to first [obtain a LevelUp Sandbox Environment API key](https://www.thelevelup.com/developer/sandbox/apps). You will need this API key to make authentication calls to the LevelUp platform.

The Example App has a default API key for the LevelUp Sandbox environment preloaded in the code. Once you obtain your own API key, you should overwrite the `API_KEY` value in LevelUpExampleAppConfigGlobals  and/or update the value in the `Api_Key.txt` file both of which are found in the Example App project. 

### Configuring the Environment
###### LevelUp Sandbox Environment 
By default, the Example App works in the **LevelUp Sandbox Environment**. In this environment, any orders created will not use real money and customer's accounts will not be charged. This is a great environment to do your testing in! 

###### LevelUp Production Environment
Once you are ready to move to production, you will need to [obtain a  LevelUp Production Environment API Key](https://www.thelevelup.com/developer/production/apps) and update the value in the `Api_Key.txt` file. Additionally, you will need to delete the `BaseUrl.config` file from your application directory. 

If no `BaseUrl.config` file is present, the SDK will default to the **LevelUp Production Environment**. Other Url's may be put into the `BaseUrl.config` file to aid in testing as long as they have [valid formatting](https://en.wikipedia.org/wiki/Uniform_Resource_Locator#Syntax).

##### !! WARNING !!
If you create an order in the LevelUp Production Environment, real money will change hands and any relevant accounts will be charged. Conversely, if you deploy a solution that is pointed at the LevelUp Sandbox Environment, no money will be paid.

### Building SDK & Example App
- Open the "LevelUp.Sdk.sln" solution file in Visual Studio
- Right click on the solution element in the Solution Explorer
- Select "Rebuild Solution" from the drop down menu
- You may ignore the error that says `The command "xcopy /Y "%PathToSdkFolder%\LevelUp.Api.Client.Test\TestData\test_config_settings.xml" "%PathToSdkFolder%\LevelUp.Api.Client.Test\bin\Net40\"" exited with code 4.` for the moment. It will become relevant for running the unit tests.

### Running the Example App
- Open the "LevelUp.Sdk.sln" solution file in Visual Studio
- Right click the ExampleApp project in the Solution Explorer and select "Set As Startup Project" from the drop-down menu
- Select the "DEBUG" menu from the menu bar at the top of the solution window then select "Start Debugging" from the drop-down

### Running the Automated Tests
Several automated tests have been written to ensure the continued quality of the LevelUp C# Integration SDK. We have included the source code and projects for these tests so that you may inspect, augment, and run them if you like. To be able to run them, you must modify the file named `%PathToSdkFolder%\LevelUpIntegrationSdk\LevelUp.Api.Client.Test\TestData\test_config_settings.xml.example` in a text editor and fill in the fields with your account information. Then save the file and rename it to "test_config_settings.xml". The fields you will have to modify are as follows: API Key, Account Username, Account Password, & Account Email Address. You may also, optionally, update the QR Code Payment Token, QR Code Payment Token with 10% tip encoded, QR Code Payment Token for an anonymous gift card user account, & QR Code that is not a LevelUp payment token.

Now you should be able to run the unit tests for this project by opening the TestExplorer window within Visual Studio and selecting the Run All option.

We *strongly recommend* that you perform your unit testing in the **LevelUp Sandbox Environment** to minimize  unexpected transfer of wealth.