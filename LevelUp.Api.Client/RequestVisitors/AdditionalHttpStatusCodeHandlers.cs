#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="AdditionalHttpStatusCodeHandlers.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.RequestVisitors
{
    /// <summary>
    /// A visitor for IRequest objects that knows, for each type of object, if there
    /// are any special behaviors required for a particular http status code. This 
    /// allows custom handlers for particular status codes that may be returned from 
    /// an api request.  This class is the default visitor that gets used in the 
    /// implementation of the LevelUp client interfaces.
    /// </summary>
    /// <remarks>
    /// Motivation: Some requests may require special behavior in the case of a 
    /// particular returned status code. The default behavior is to throw a 
    /// LevelUpApiException any time platform returns a response other than 200 or 
    /// 204.  But, for instance, if the client wanted to log something special when 
    /// a LocationDetailsQueryRequest returns a 404, they could do that here.
    /// 
    /// This is the default class that gets used in the implementation of the 
    /// client interfaces.  However, a 3rd party client could theoretically sub-in 
    /// a different (or derived) visitor if they wanted to change or add a particular 
    /// action for a given type of status code. They would do so by providing the 
    /// alternative visitor during the creation of the LevelUpClient's request 
    /// execution engine.
    /// </remarks>
    public class AdditionalHttpStatusCodeHandlers : RequestVisitorWithDefault<Dictionary<HttpStatusCode, LevelUpRestWrapper.ResponseAction>>
    {
        private static readonly Func<Request, Dictionary<HttpStatusCode, LevelUpRestWrapper.ResponseAction>> DEFAULT_FUNCTION =
            (request) => IHaveNoCustomHttpResponseHandlers();

        public AdditionalHttpStatusCodeHandlers()
            : base(DEFAULT_FUNCTION)
        { }

        public override Dictionary<HttpStatusCode, LevelUpRestWrapper.ResponseAction> Visit(Models.Requests.LocationDetailsQueryRequest request)
        {
            // Special case 404 error since this means the location does not exist, is not visible, 
            // or the merchant owner of the location is not live
            return new Dictionary<HttpStatusCode, LevelUpRestWrapper.ResponseAction>
            {
                {
                    HttpStatusCode.NotFound,
                    response =>
                    {
                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            throw new LevelUpApiException(string.Format("Cannot get location details for location {0}." +
                                                                        " This location may not exist, not be visible," +
                                                                        " or the merchant owner may not be live.",
                                                                        request.LocationId),
                                                          response.StatusCode,
                                                          response.ErrorException);
                        }
                    }
                }
            };
        }

        private static Dictionary<HttpStatusCode, LevelUpRestWrapper.ResponseAction> IHaveNoCustomHttpResponseHandlers()
        {
            return new Dictionary<HttpStatusCode, LevelUpRestWrapper.ResponseAction>();
        }
    }
}
