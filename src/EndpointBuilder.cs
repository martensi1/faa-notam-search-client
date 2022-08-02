using System;
using System.Linq;

namespace PilotAppLib.Clients.NotamSearch
{
    interface IEndpointBuilder
    {
        public string BuildHttpEndpoint(string[] airports);
    }

    class EndpointBuilder : IEndpointBuilder
    {
        private const string BaseUrl = "https://notams.aim.faa.gov/notamSearch/search";


        public string BuildHttpEndpoint(string[] airports)
        {
            if (airports == null)
                throw new ArgumentNullException(nameof(airports));

            return BaseUrl
                    + $"?searchType=0"
                    + $"&designatorsForLocation={string.Join(",", airports)}";
        }
    }
}
