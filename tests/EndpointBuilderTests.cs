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
        [InlineData(new[] { "ESGJ" }, "searchType=0&designatorsForLocation=ESGJ")]
        [InlineData(new[] { "ESGJ", "ESSA" }, "searchType=0&designatorsForLocation=ESGJ,ESSA")]
        [InlineData(new[] { "ESGJ", "ESSA", "ESMX" }, "searchType=0&designatorsForLocation=ESGJ,ESSA,ESMX")]
        public void BuildEndpoint(string[] airportIcaos, string expectedUrlArguments)
        {
            string output = _builder.BuildHttpEndpoint(airportIcaos);
            Assert.Equal($"{ApiBaseUrl}?{expectedUrlArguments}", output);
        }
    }
}
