using System.Collections.Generic;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp response to a MangedLocationQuery request
    /// </summary>
    public class ManagedLocationQueryResponse : IResponse
    {
        public readonly List<ManagedLocation> Details;

        public ManagedLocationQueryResponse(List<ManagedLocation> details)
        {
            Details = details;
        }
    }
}
