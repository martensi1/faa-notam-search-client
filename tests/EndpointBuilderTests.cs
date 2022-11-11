using System;
using Xunit;

namespace PilotAppLib.Clients.NotamSearch.Tests
{
    public class EndpointBuilderTests
    {
        private const string ApiBaseUrl = "https://notams.aim.faa.gov/notamSearch/search";
        private readonly EndpointBuilder _builder;


        public EndpointBuilderTests()
        {
            _builder = new EndpointBuilder();
        }

        
        [Theory]
        [InlineData(new[] { "ESGJ" }, 0, "searchType=0&designatorsForLocation=ESGJ&offset=0")]
        [InlineData(new[] { "ESGJ", "ESSA" }, 1, "searchType=0&designatorsForLocation=ESGJ,ESSA&offset=1")]
        [InlineData(new[] { "ESGJ", "ESSA", "ESMX" }, 2, "searchType=0&designatorsForLocation=ESGJ,ESSA,ESMX&offset=2")]
        public void BuildEndpoint(string[] airportIcaos, uint offset, string expectedUrlArguments)
        {
            string output = _builder.BuildHttpEndpoint(airportIcaos, offset);
            Assert.Equal($"{ApiBaseUrl}?{expectedUrlArguments}", output);
        }
    }
}
