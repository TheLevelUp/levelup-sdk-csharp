#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpUriBuilder.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Http
{
    public class LevelUpUriBuilder
    {
        private readonly UriBuilder _uriBuilder;

        public LevelUpUriBuilder(LevelUpEnvironment environment)
        {
            Environment = environment;
            _uriBuilder = new UriBuilder(environment.ToString());
        }

        public LevelUpApiVersion ApiVersion { get; private set; }

        public LevelUpEnvironment Environment { get; }

        public string Path { get; private set; }

        public LevelUpUriBuilder AppendQuery(string key, string value)
        {
            string queryToAppend = $"{key}={value}";
            _uriBuilder.Query = ConcatUriQueries(_uriBuilder.Query, queryToAppend);
            return this;
        }

        public string Build(bool includePortNumber = false)
        {
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

        public LevelUpUriBuilder SetPath(string path)
        {
            Path = path;
            ClearQueries();
            return this;
        }

        private string ConcatUriQueries(string original, string toAppend)
        {
            if (string.IsNullOrEmpty(toAppend) || original.Contains(toAppend))
            {
                return original;
            }

            if (string.IsNullOrEmpty(original))
            {
                return toAppend;
            }

            return $"{original}&{toAppend}";
        }
    }
}
