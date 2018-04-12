#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpApiException.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Linq;
using System.Net;

namespace LevelUp.Api.Http
{
    public class LevelUpApiException : Exception
    {
        public LevelUpApiException(string message = "", Exception innerException = null)
            : base(message, innerException)
        {
        }

        public LevelUpApiException(string message, HttpStatusCode status, Exception innerException = null)
            : this(message, innerException)
        {
            StatusCode = status;
        }

        public LevelUpApiException(List<ErrorResponse> message, HttpStatusCode status, Exception innerException = null)
            : this(string.Join(Environment.NewLine, message?.Select(x => x.Message).ToArray() ?? new string[] { }), innerException)
        {
            StatusCode = status;
        }

        public HttpStatusCode StatusCode { get; }

        public override string ToString()
        {
            return string.Format("{0} Http Status: {1} [{2}]", base.ToString(), StatusCode, (int)StatusCode);
        }
    }
}
