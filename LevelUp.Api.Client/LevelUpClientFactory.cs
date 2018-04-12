#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpClientFactory.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client
{
    public class LevelUpClientFactory
    {
        private LevelUpClientFactory() { }

        /// <summary>
        /// Create a LevelUp client interface
        /// </summary>
        /// <typeparam name="T">The type of LevelUp client interface to generate.</typeparam>
        /// <param name="identifier">An object that retains data about the caller 
        /// (company name, etc.) as an informational and diagnostic tool for the 
        /// LevelUp backend.</param>
        /// <param name="enviornment">The target enviornment (sandbox, production, etc.)</param>
        /// <returns>The LevelUp client interface of the specified type T.</returns>
        public static T Create<T>(AgentIdentifier identifier, LevelUpEnvironment enviornment) 
            where T : ILevelUpClientModule
        {
            IRestfulService httpRestService = new LevelUpHttpRestfulService();
            var client = new LevelUpClient(httpRestService, identifier, enviornment);
            return (T)(object)client;
        }

        /// <summary>
        /// Create a composed interface, containing a collection of the more fine-grained
        /// LevelUp client interfaces.
        /// </summary>
        /// <typeparam name="T">The type of composed interface to generate.</typeparam>
        /// <param name="identifier">An object that retains data about the caller 
        /// (company name, etc.) as an informational and diagnostic tool for the 
        /// LevelUp backend.</param>
        /// <param name="enviornment">The target enviornment (sandbox, production, etc.)</param>
        /// <returns>The LevelUp client interface of the specified type T.</returns>
        [System.Obsolete("CreateComposedInterface<T>(...) is deprecated; please use Create<T>(...) instead.")]
        public static T CreateComposedInterface<T>(AgentIdentifier identifier, LevelUpEnvironment enviornment)
            where T : IComposedInterface
        {
            IRestfulService httpRestService = new LevelUpHttpRestfulService();
            var client = new LevelUpClient(httpRestService, identifier, enviornment);
            return (T)(object)client;
        }

        /// <summary>
        /// For Unit Testing, allowing mocking of the LevelUpHttpRestfulService.  Note 
        /// that the AssemblyInfo.cs file enables internals to be visible to the unit 
        /// test project.
        /// </summary>
        internal static T Create<T>(AgentIdentifier identifier, LevelUpEnvironment enviornment, IRestfulService restService)
            where T : ILevelUpClientModule
        {
            var client = new LevelUpClient(restService, identifier, enviornment);
            return (T) (object) client;
        }
    }
}
