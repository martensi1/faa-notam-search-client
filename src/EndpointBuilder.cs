using System;

namespace PilotAppLib.Clients.NotamSearch
{
    interface IEndpointBuilder
    {
        public string BuildHttpEndpoint(string[] airports, uint offset);
    }

    class EndpointBuilder : IEndpointBuilder
    {
        private const string BaseUrl = "https://notams.aim.faa.gov/notamSearch/search";


        public string BuildHttpEndpoint(string[] airports, uint offset)
        {
            if (airports == null)
                throw new ArgumentNullException(nameof(airports));

            return BaseUrl
                    + $"?searchType=0"
                    + $"&designatorsForLocation={string.Join(",", airports)}"
                    + $"&offset={offset}";
        }
    }
}
