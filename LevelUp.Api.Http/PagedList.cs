#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="PagedList.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RestSharp;

namespace LevelUp.Api.Client.Models.Responses
{
    public class PagedList<T> : IEnumerable<T>
    {
        public delegate IRestResponse EndpointInvoker(string nextPageUrl);
        private EndpointInvoker _endpointInvoker;
        private List<T> _items;
        private string _nextPageUrl;

        private PagedList() { }

        /// <summary>
        /// Creates a paginated list for the result of "List" endpoint calls to the LevelUp API.  
        /// </summary>
        /// <param name="endpointInvoker">A delegate to call the LevelUp endpoint to get next page of results.</param>
        /// <param name="pageUrl">The URL of the next page of results</param>
        /// <param name="currentPage">The number of the current page of results</param>
        public PagedList(EndpointInvoker endpointInvoker,
                         string pageUrl,
                         int currentPage)
        {
            _endpointInvoker = endpointInvoker;
            CurrentPage = currentPage;

            IRestResponse response = _endpointInvoker(CreatePageUrl(pageUrl, currentPage));

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                _nextPageUrl = string.Empty;
                _items = new List<T>();
            }
            else
            {
                _items = JsonConvert.DeserializeObject<List<T>>(response.Content);
                _nextPageUrl = ParseNextPageUrl(response.Headers);
            }
        }

        /// <summary>
        /// The count of items in the the current page of the list.
        /// </summary>
        public int Count
        {
            get
            {
                return (null == _items) ? 0 : _items.Count;
            }
        }

        /// <summary>
        /// The current page number of the list.
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        /// Returns true if there are more results in the next page of the list.
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return !string.IsNullOrEmpty(_nextPageUrl);
            }
        }

        /// <summary>
        /// Gets the next page of results in the list.  Throws an InvalidOperationException if there are no more pages.
        /// </summary>
        public PagedList<T> NextPage()
        {
            if (!HasNextPage)
            {
                throw new InvalidOperationException("No new pages found.");
            }

            return new PagedList<T>(endpointInvoker: _endpointInvoker,
                                    currentPage: CurrentPage + 1,
                                    pageUrl: _nextPageUrl);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string CreatePageUrl(string pageUrl, int currentPage)
        {
            const string PAGE_OPTION_FORMAT = "page=";

            if (pageUrl.Contains(PAGE_OPTION_FORMAT))
            {
                return pageUrl;
            }

            System.Text.StringBuilder urlBuilder = new System.Text.StringBuilder(pageUrl);

            const string firstArgumentString = "?";

            if (!pageUrl.Contains(firstArgumentString))
            {
                urlBuilder.Append(firstArgumentString);
            }
            else
            {
                const string additionalArgumentsString = "&";

                urlBuilder.Append(additionalArgumentsString);
            }

            urlBuilder.Append(PAGE_OPTION_FORMAT);
            urlBuilder.Append(currentPage);

            return urlBuilder.ToString();
        }

        /// <summary>
        /// Given a set of headers from a LevelUp response, returns the URL for the next page of results.
        /// </summary>
        /// <param name="responseHeadersToParse"></param>
        /// <returns></returns>
        private static string ParseNextPageUrl(IEnumerable<Parameter> responseHeadersToParse)
        {
            // Link header format is '<nextPageURL>; rel="next"'
            // Regex pattern matches the url between the brackets as long as it is not whitespace
            const string LINK_HEADER_REGEX_MATCH_PATTERN = @"(?<=\<)[^\s]*(?=\>)";
            Regex NEXT_PAGE_REG_EX = new Regex(LINK_HEADER_REGEX_MATCH_PATTERN);

            foreach (Parameter p in responseHeadersToParse)
            {
                if ("Link".Equals(p.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    string linkHeader = p.Value as string;
                    if (!string.IsNullOrEmpty(linkHeader))
                    {
                        return NEXT_PAGE_REG_EX.Match(linkHeader).Value;
                    }
                }
            }

            return string.Empty;
        }
    }
}
