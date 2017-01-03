using System;
using System.Collections.Generic;
using System.Text;
using LevelUp.Api.Client.Models.RequestVisitors;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Request to retrieve information about a user's locations.  This is distinct from the 
    /// MerchantLocationQueryRequest in that it only shows the locations that the authenticated 
    /// user manages.  A franchise merchant may have hundreds of assoicated locations, however
    /// if a franchisee would like to only get data for the two stores they manage, this 
    /// request would be ideal.
    /// </summary>
    public class ManagedLocationQueryRequest : Request
    {
        protected override LevelUpApiVersion _applicableAPIVersionsBitmask
        {
            get { return LevelUpApiVersion.v15; }
        }

        public ManagedLocationQueryRequest(string accessToken) : base (accessToken) { }

        /// <summary>
        /// Acceptance method for Request visitors.
        /// </summary>
        public override T Accept<T>(IRequestVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
