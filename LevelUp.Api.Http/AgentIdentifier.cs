#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="AgentIdentifier.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Text;

namespace LevelUp.Api.Http
{
    public class AgentIdentifier
    {
        private string _companyName = string.Empty;
        private string _productName = string.Empty;
        private string _productVersion = string.Empty;
        private string _osName = string.Empty;

        /// <summary>
        /// Class that holds identifying information for the individual or group who is developing against the 
        /// LevelUp API. This information may be used to more easily identify the source of requests made to LevelUp.
        /// This information will be sent as part of each request to LevelUp in the User-Agent header field.
        /// </summary>
        /// <param name="companyName">Name of the individual, group, or company name developing this software</param>
        /// <param name="productName">Name of the product or tool into which this software will be built</param>
        /// <param name="productVersion">Version of the product or tool of which this software is a part</param>
        /// <param name="osName">Name of the operating system on which this software runs</param>
        public AgentIdentifier(string companyName,
                               string productName,
                               string productVersion,
                               string osName)
        {
            this.CompanyName = companyName;
            this.ProductName = productName;
            this.ProductVersion = productVersion;
            this.OsName = FormatOsName(osName);
        }

        /// <summary>
        /// Name of the individual, group, or company developing this software
        /// </summary>
        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = FlattenString(value); }
        }

        /// <summary>
        /// Name of the product or tool of which this software is a part
        /// </summary>
        public string ProductName
        {
            get { return _productName; }
            set { _productName = FlattenString(value); }
        }

        /// <summary>
        /// Version of the product or tool of which this software is a part
        /// </summary>
        public string ProductVersion
        {
            get { return _productVersion; }
            set { _productVersion = FlattenString(value); }
        }

        /// <summary>
        /// Name of the operating system on which this software runs
        /// </summary>
        public string OsName
        {
            get { return _osName; }
            set { _osName = FlattenString(value); }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            //Desired format: "CompanyName-ProductName/ProductVersion [OsName]" from https://tools.ietf.org/html/rfc2616#section-14.43
            if (!string.IsNullOrEmpty(CompanyName))
            {
                sb.AppendFormat("{0}", CompanyName);
            }

            if (!string.IsNullOrEmpty(ProductName))
            {
                if (sb.Length > 0)
                {
                    sb.Append("-");
                }

                sb.AppendFormat("{0}", ProductName);

                if (!string.IsNullOrEmpty(ProductVersion))
                {
                    sb.AppendFormat("/{0}", ProductVersion);
                }
            }

            if (!string.IsNullOrEmpty(OsName))
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }

                sb.AppendFormat("[{0}]", OsName);
            }

            return sb.ToString();
        }

        private static string FlattenString(string stringToFlatten)
        {
            if (string.IsNullOrEmpty(stringToFlatten))
            {
                return stringToFlatten;
            }

            string trimmed = stringToFlatten.Trim();

            return trimmed.Replace(Environment.NewLine, "¶");
        }

        private static string FormatOsName(string osName)
        {
            string osVersion = "UNKNOWN OS VERSION";

            try
            {
                osVersion = Environment.OSVersion.VersionString;
            }
            catch (InvalidOperationException)
            {
            }

            return string.IsNullOrEmpty(osName) ? osVersion : $"{osName} | {osVersion}";
        }
    }
}
