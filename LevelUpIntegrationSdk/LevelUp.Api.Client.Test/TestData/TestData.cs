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
        public const string DEFAULT_PATH_TO_URI_CONFIG = "LevelUpBaseUri.config";

        internal static class Valid
        {
            public const string COMPANY_NAME = "LevelUp POS Team";
            public const string PRODUCT_NAME = "C# Integration SDK";
            public const string PRODUCT_VERSION = "0.1";
            public const string OS_NAME = "Windows, .NET 3.0+";
            public const int POS_MERCHANT_ID = 3225;
            public const int POS_LOCATION_ID = 3796;
            public const int POS_TEST_USER_ID = 231564;
            public const string POS_TEST_USER_FIRST_NAME = "Test";
            public const string POS_TEST_USER_LAST_INITIAL = "U";

            public const string CONTRIBUTION_TARGET_ID = "5";

            public const int INVISIBLE_LOCATION_ID = 3796;
            public const int VISIBLE_LOCATION_ID = 7929;
            public const int VISIBLE_MERCHANT_ID = 5869;
            public const string VISIBLE_LOCATION_MERCHANT_NAME = "Grass Roots Cafe";
        }

        internal static class Invalid
        {
            public const string QR_CODE = "LU02000THISISNOTAVALIDCODE00A0LU";
        }
    }
}
