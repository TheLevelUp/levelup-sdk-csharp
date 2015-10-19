//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpClientFactory.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Http;

namespace LevelUp.Api.Client
{
    public class LevelUpClientFactory
    {
        private LevelUpClientFactory()
        {
        }

        public static ILevelUpClient Create(string companyName,
                                            string productName,
                                            string productVersion,
                                            string osName)
        {
            return Create(new AgentIdentifier(companyName, productName, productVersion, osName));
        }

        public static ILevelUpClient Create(string companyName,
                                            string productName,
                                            string productVersion,
                                            string osName,
                                            string pathToConfigFile)
        {
            return Create(new AgentIdentifier(companyName, productName, productVersion, osName), pathToConfigFile);
        }

        public static ILevelUpClient Create(AgentIdentifier identifier)
        {
            return new LevelUpClient(identifier, new LevelUpRestWrapper());
        }

        public static ILevelUpClient Create(AgentIdentifier identifier, string pathToConfigFile)
        {
            return new LevelUpClient(identifier, new LevelUpRestWrapper(), pathToConfigFile);
        }
    }
}
