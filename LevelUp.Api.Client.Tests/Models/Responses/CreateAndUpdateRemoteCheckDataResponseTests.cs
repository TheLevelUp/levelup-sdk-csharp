#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreateAndUpdateRemoteCheckDataResponseTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Diagnostics;
using FluentAssertions;
using LevelUp.Api.Client.Models.Responses;
using NUnit.Framework;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Tests.Models.Responses
{
    [TestFixture]
    public class CreateAndUpdateRemoteCheckDataResponseTests : ModelTestsBase
    {
        private const string LOCATOR = "AB43HJ";

        private const string LOCATOR_QR_CODE =
            "\\u00A0\\u2584\\u2584\\u2584\\u2584\\u2584\\u2584\\u2584\\u00A0\\u2584\\u2584\\u00A0\\u00A0" +
            "\\u2584\\u00A0\\u2584\\u2584\\u2584\\u2584\\u2584\\u2584\\u2584\\u00A0" +
            ",\\u00A0\\u2588\\u00A0\\u2584\\u2584\\u2584\\u00A0\\u2588\\u00A0\\u2584\\u2580\\u2584\\u00A0" +
            "\\u2588\\u00A0\\u2588\\u00A0\\u2584\\u2584\\u2584\\u00A0\\u2588\\u00A0" +
            ",\\u00A0\\u2588\\u00A0\\u2588\\u2588\\u2588\\u00A0\\u2588\\u00A0\\u2588\\u2584\\u2584\\u2580" +
            "\\u00A0\\u00A0\\u2588\\u00A0\\u2588\\u2588\\u2588\\u00A0\\u2588\\u00A0" +
            ",\\u00A0\\u2588\\u2584\\u2584\\u2584\\u2584\\u2584\\u2588\\u00A0\\u2584\\u00A0\\u2584\\u00A0" +
            "\\u2584\\u00A0\\u2588\\u2584\\u2584\\u2584\\u2584\\u2584\\u2588\\u00A0" +
            ",\\u00A0\\u2584\\u2584\\u2584\\u2584\\u00A0\\u00A0\\u2584\\u00A0\\u2584\\u2580\\u2588\\u00A0" +
            "\\u00A0\\u2584\\u00A0\\u00A0\\u2584\\u2584\\u2584\\u00A0\\u2584\\u00A0" +
            ",\\u00A0\\u2584\\u2584\\u2588\\u2580\\u2584\\u2580\\u2584\\u2588\\u2580\\u2584\\u2580\\u00A0" +
            "\\u2588\\u2584\\u2584\\u2588\\u2584\\u00A0\\u2580\\u2588\\u00A0\\u00A0" +
            ",\\u00A0\\u2580\\u2588\\u2584\\u2580\\u2580\\u2588\\u2584\\u2588\\u2580\\u2580\\u2584\\u2584" +
            "\\u2580\\u2588\\u00A0\\u00A0\\u2584\\u2580\\u00A0\\u2588\\u2584\\u00A0" +
            ",\\u00A0\\u2584\\u2584\\u2584\\u2584\\u2584\\u2584\\u2584\\u00A0\\u2580\\u2588\\u2580\\u00A0" +
            "\\u00A0\\u2588\\u2584\\u00A0\\u2584\\u00A0\\u2588\\u2588\\u2580\\u00A0" +
            ",\\u00A0\\u2588\\u00A0\\u2584\\u2584\\u2584\\u00A0\\u2588\\u00A0\\u00A0\\u2588\\u00A0\\u2588" +
            "\\u2580\\u2580\\u2588\\u2580\\u2584\\u2588\\u00A0\\u2584\\u2584\\u00A0" +
            ",\\u00A0\\u2588\\u00A0\\u2588\\u2588\\u2588\\u00A0\\u2588\\u00A0\\u2588\\u2588\\u2588\\u2584" +
            "\\u00A0\\u2588\\u00A0\\u2584\\u2584\\u2580\\u2584\\u2580\\u00A0\\u00A0" +
            ",\\u00A0\\u2588\\u2584\\u2584\\u2584\\u2584\\u2584\\u2588\\u00A0\\u2588\\u00A0\\u2584\\u2588" +
            "\\u2588\\u00A0\\u2580\\u00A0\\u00A0\\u2580\\u00A0\\u2584\\u00A0\\u00A0" +
            ",\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0" +
            "\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0\\u00A0";

        //Alternate format using Extended ASCII codes from Code Page 437
        //"255 220 220 220 220 220 220 220 255 220 220 255 255 220 255 220 220 " +
        //"220 220 220 220 220 255" +
        //",255 219 255 220 220 220 255 219 255 220 223 220 255 219 255 219 255 " +
        //"220 220 220 255 219 255" +
        //",255 219 255 219 219 219 255 219 255 219 220 220 223 255 255 219 255 " +
        //"219 219 219 255 219 255" +
        //",255 219 220 220 220 220 220 219 255 220 255 220 255 220 255 219 220 " +
        //"220 220 220 220 219 255" +
        //",255 220 220 220 220 255 255 220 255 220 223 219 255 255 220 255 255 " +
        //"220 220 220 255 220 255" +
        //",255 220 220 219 223 220 223 220 219 223 220 223 255 219 220 220 219 " +
        //"220 255 223 219 255 255" +
        //",255 223 219 220 223 223 219 220 219 223 223 220 220 223 219 255 255 " +
        //"220 223 255 219 220 255" +
        //",255 220 220 220 220 220 220 220 255 223 219 223 255 255 219 220 255 " +
        //"220 255 219 219 223 255" +
        //",255 219 255 220 220 220 255 219 255 255 219 255 219 223 223 219 223 " +
        //"220 219 255 220 220 255" +
        //",255 219 255 219 219 219 255 219 255 219 219 219 220 255 219 255 220 " +
        //"220 223 220 223 255 255" +
        //",255 219 220 220 220 220 220 219 255 219 255 220 219 219 255 223 255 " +
        //"255 223 255 220 255 255" +
        //",255 255 255 255 255 255 255 255 255 255 255 255 255 255 255 255 255 " +
        //"255 255 255 255 255 255";

        //DO NOT MODIFY unless you really know what you are doing. This string contains non-printing chars
        // That look very much like spaces but are not equal to the normal space char.
        private static readonly string[] EXPECTED_QR_CODE_LINES = new string[]
            {
                " ▄▄▄▄▄▄▄ ▄▄  ▄ ▄▄▄▄▄▄▄ ",
                " █ ▄▄▄ █ ▄▀▄ █ █ ▄▄▄ █ ",
                " █ ███ █ █▄▄▀  █ ███ █ ",
                " █▄▄▄▄▄█ ▄ ▄ ▄ █▄▄▄▄▄█ ",
                " ▄▄▄▄  ▄ ▄▀█  ▄  ▄▄▄ ▄ ",
                " ▄▄█▀▄▀▄█▀▄▀ █▄▄█▄ ▀█  ",
                " ▀█▄▀▀█▄█▀▀▄▄▀█  ▄▀ █▄ ",
                " ▄▄▄▄▄▄▄ ▀█▀  █▄ ▄ ██▀ ",
                " █ ▄▄▄ █  █ █▀▀█▀▄█ ▄▄ ",
                " █ ███ █ ███▄ █ ▄▄▀▄▀  ",
                " █▄▄▄▄▄█ █ ▄██ ▀  ▀ ▄  ",
                "                       ",
            };

        [Test]
        
        public void Deserialize()
        {
            string uuid = Guid.NewGuid().ToString("N");

            UpdateRemoteCheckDataResponse response = null;

            response.Should().BeNull();

            string json = CreateSerializedObjectJsonString(LOCATOR, uuid, LOCATOR_QR_CODE);

            response = JsonConvert.DeserializeObject<UpdateRemoteCheckDataResponse>(json);

            response.Should().NotBeNull();
            response.CheckLocator.Should().BeEquivalentTo(LOCATOR);
            response.CheckIdentifier.Should().BeEquivalentTo(uuid);
            response.QrCodeSymbols.Should().NotBeNullOrEmpty();

            response.QrCodeSymbols.GetLength(0).Should().Be(EXPECTED_QR_CODE_LINES.GetLength(0));
            Trace.WriteLine(string.Join(Environment.NewLine, response.QrCodeSymbols));

            for (int i = 0; i < response.QrCodeSymbols.Length; i++)
            {
                response.QrCodeSymbols[i].Should().BeEquivalentTo(EXPECTED_QR_CODE_LINES[i], "Line {0}", i);
            }
        }

        private string CreateSerializedObjectJsonString(string locator, string uuid, string asciiQrCode)
        {
            return "{\"check\":{\"locator\":" + FormatJsonString(locator) +
                   ",\"uuid\":" + FormatJsonString(uuid) +
                   ",\"qr_code_character_art\":" + FormatJsonString(asciiQrCode) + "}}";
        }
    }
}
