//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="TestData.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Test
{
    internal static class TestData
    {
        internal static class Valid
        {
            public const string COMPANY_NAME = "LevelUp POS Team";
            public const string PRODUCT_NAME = "C# Integration SDK";
            public const string PRODUCT_VERSION = "2.0";
            public const string OS_NAME = "Windows, .NET 3.0+";

            public const string CONTRIBUTION_TARGET_ID = "5";
        }

        internal static class Invalid
        {
            public const string QR_CODE = "LU02000THISISNOTAVALIDCODE00A0LU";
        }
    }
}
