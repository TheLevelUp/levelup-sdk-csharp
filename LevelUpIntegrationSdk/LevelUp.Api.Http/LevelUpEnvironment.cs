//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpEnvironment.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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


using System;

namespace LevelUp.Api.Http
{
    public class LevelUpEnvironment
    {
        public static LevelUpEnvironment Production
        {
            get { return new LevelUpEnvironment(@"https://api.thelevelup.com"); }
        }

        public static LevelUpEnvironment Sandbox
        {
            get { return new LevelUpEnvironment(@"https://sandbox.thelevelup.com"); }
        }

        public static LevelUpEnvironment Staging
        {
            get { return new LevelUpEnvironment(@"https://staging.thelevelup.com"); }
        }

        private readonly string _baseUri;

        private LevelUpEnvironment(string baseUri)
        {
            _baseUri = baseUri;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LevelUpEnvironment))
            {
                return false;
            }

            return Equals(obj as LevelUpEnvironment);
        }

        protected bool Equals(LevelUpEnvironment other)
        {
            return string.Equals(_baseUri, other._baseUri, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (_baseUri != null ? _baseUri.GetHashCode() : 0);
        }

        public static bool operator ==(LevelUpEnvironment left, LevelUpEnvironment right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LevelUpEnvironment left, LevelUpEnvironment right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return _baseUri;
        }
    }
}
