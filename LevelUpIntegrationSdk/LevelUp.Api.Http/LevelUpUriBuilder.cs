//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpUriBuilder.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2014 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using System;

namespace LevelUp.Api.Http
{
    public class LevelUpUriBuilder
    {
        private const string HTTP_PREFIX = "http://";
        private const string HTTPS_PREFIX = "https://";
        private const char PORT_SEPARATOR_CHAR = ':';

        private UriBuilder _uriBuilder;

        public LevelUpUriBuilder() : this(LevelUpEnvironment.Production) { }

        public LevelUpUriBuilder(LevelUpEnvironment environment)
        {
            this.SetEnvironment(environment);
        }

        public LevelUpUriBuilder(string baseUri)
        {
            Uri uri;
            int? portNum = null;

            //If the base URI passed does not include either an HTTP or HTTPS prefix, add it in
            if (!(baseUri.StartsWith(HTTP_PREFIX) || baseUri.StartsWith(HTTPS_PREFIX)))
            {
                baseUri = string.Concat(HTTPS_PREFIX, baseUri);
            }

            //Search the portion of the base URI that comes after the HTTP or HTTPS prefix for a colon which 
            //should indicate a port number was specified
            if (baseUri.Substring(HTTPS_PREFIX.Length).Contains(PORT_SEPARATOR_CHAR.ToString()))
            {
                string portNumStr = baseUri.Substring(baseUri.LastIndexOf(PORT_SEPARATOR_CHAR) + 1);

                try
                {
                    portNum = int.Parse(portNumStr);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(string.Format("Unexpected base URI format: {0}!", baseUri), ex);
                }
            }

            if (!Uri.TryCreate(baseUri, UriKind.Absolute, out uri))
            {
                throw new ArgumentException(string.Format("\"{0}\" is not a valid URI.", baseUri));
            }

            _uriBuilder = new UriBuilder(uri);

            if (portNum.HasValue)
            {
                _uriBuilder.Port = portNum.Value;
            }
        }

        public LevelUpUriBuilder AppendQuery(string key, string value)
        {
            string queryToAppend = string.Format("{0}={1}", key, value);

            if (_uriBuilder.Query != null &&
                _uriBuilder.Query.Length > 1 && 
                !_uriBuilder.Query.Contains(queryToAppend))
            {
                //Append the query string to the end of the query. See https://msdn.microsoft.com/query/dev11.query?appId=Dev11IDEF1&l=EN-US&k=k(System.UriBuilder.Query);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv3.0);k(DevLang-csharp)&rd=true
                _uriBuilder.Query = string.Format("{0}&{1}", _uriBuilder.Query.Substring(1), queryToAppend);
            }
            else
            {
                _uriBuilder.Query = queryToAppend;
            }

            return this;
        }

        public string Build(bool includePortNumber = false)
        {
            if (null == _uriBuilder)
            {
                throw new InvalidOperationException("Invalid Object State! _uriBuilder is null.");
            }

            if (string.IsNullOrEmpty(Path))
            {
                throw new InvalidOperationException("Path must be set!");
            }

            _uriBuilder.Path = string.Format("/{0}/{1}", Enum.GetName(typeof(LevelUpApiVersion), ApiVersion), Path);

            return includePortNumber ? _uriBuilder.ToString() : _uriBuilder.Uri.ToString();
        }

        public LevelUpUriBuilder ClearQueries()
        {
            _uriBuilder.Query = string.Empty;
            return this;
        }

        public LevelUpUriBuilder SetApiVersion(LevelUpApiVersion version)
        {
            ApiVersion = version;
            return this;
        }

        public LevelUpUriBuilder SetEnvironment(LevelUpEnvironment environment)
        {
            Environment = environment;
            _uriBuilder = new UriBuilder(environment.ToString());
            return this;
        }

        public LevelUpUriBuilder SetPath(string path)
        {
            Path = path;

            //If we set the path, we should clear out the queries
            ClearQueries();

            return this;
        }

        public LevelUpApiVersion ApiVersion { get; private set; }

        public LevelUpEnvironment Environment { get; private set; }

        public string Host { get { return _uriBuilder.Host; } }

        public string Path { get; private set; }

        public string Query { get { return _uriBuilder.Query; } }

        public string Scheme { get { return _uriBuilder.Scheme; } }
    }
}
