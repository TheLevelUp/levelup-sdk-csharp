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

using System;
using System.Collections.Generic;
using System.Net;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.RequestVisitors;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Client.RequestVisitors;
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
            var client = new LevelUpClient(GetDefaultLevelUpRequestEngine(identifier, enviornment, httpRestService));
            return (T)(object)client;
        }

        /// <summary>
        /// Create a LevelUp client interface with a custom request engine
        /// </summary>
        /// <remarks>
        /// Use this only if you need to override certain behaviors in the way that 
        /// the SDK makes calls into the levelup platform.  Otherwise, use the 
        /// standard Create<T> overload, which will use the default LevelUp 
        /// implementation classes.
        /// </remarks>
        /// <typeparam name="T">The type of LevelUp client interface to generate.</typeparam>
        /// <param name="engine">An IRequestVisitor that can make calls to the 
        /// LevelUp API for each type of request object.</param>
        /// <returns>The LevelUp client interface of the specified type T.</returns>
        public static T Create<T>(IRequestVisitor<IResponse> engine)
            where T : ILevelUpClientModule
        {
            var client = new LevelUpClient(engine);
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
        public static T CreateComposedInterface<T>(AgentIdentifier identifier, LevelUpEnvironment enviornment)
            where T : IComposedInterface
        {
            IRestfulService httpRestService = new LevelUpHttpRestfulService();
            var client = new LevelUpClient(GetDefaultLevelUpRequestEngine(identifier, enviornment, httpRestService));
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
            var client = new LevelUpClient(GetDefaultLevelUpRequestEngine(identifier, enviornment, restService));
            return (T) (object) client;
        }

        

        [Obsolete("Use the smaller more specific interfaces crated by the Create<T>(...) method.")]
        public static ILevelUpClient Create(string companyName,
                                            string productName,
                                            string productVersion,
                                            string osName)
        {
            return Create(new AgentIdentifier(companyName, productName, productVersion, osName));
        }

        [Obsolete("Use the smaller more specific interfaces created by the Create<T>(...) method.")]
        public static ILevelUpClient Create(AgentIdentifier identifier)
        {
            LevelUpEnvironment enviornment = LevelUpEnvironment.Production;
            IRestfulService httpRestService = new LevelUpHttpRestfulService();
            return new LevelUpClient(GetDefaultLevelUpRequestEngine(identifier, enviornment, httpRestService));
        }


        /// <summary>
        /// Creates an IRequestVisitor<IResponse> (i.e. the "request execution engine") using the default
        /// implementation for each of the requisite visitors.  Unless a 3rd-party client wants to override 
        /// a particular visitor (in order to modify the way that the SDK creates and interperates calls
        /// to the LevelUp api) for some reason, these defaults are what should be used in the creation of
        /// the LevelUpClient implementation class.
        /// </summary>
        private static IRequestVisitor<IResponse> GetDefaultLevelUpRequestEngine(AgentIdentifier identifier,
            LevelUpEnvironment enviornment, IRestfulService httpRestService)
        {
            IRequestVisitor<string> endpointCreator = new RequestEndpointCreator(enviornment);

            IRequestVisitor<string> headerAuthorizationTokenCreator = new RequestHeaderTokenCreator();

            IRequestVisitor<Dictionary<HttpStatusCode, LevelUpRestWrapper.ResponseAction>> customHttpStatusCodeHandlers
                = new AdditionalHttpStatusCodeHandlers();

            IRequestVisitor<IResponse> requestExecutionEngine = new RequestExecutionEngine(httpRestService, identifier,
                endpointCreator, headerAuthorizationTokenCreator, customHttpStatusCodeHandlers);

            return requestExecutionEngine;
        }

    }
}
