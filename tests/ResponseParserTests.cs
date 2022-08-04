using System.Collections.Generic;
using System.IO;
using Xunit;

namespace PilotAppLib.Clients.NotamSearch.Tests
{
    public class ResponseProcessorTests
    {
        [Theory]
        [InlineData(
            "data/api-responses/esgj-esgg-essa.json",
            new[] { "ESGJ", "ESGG", "ESSA" },
            new[] { "ESGJ NOTAM MESSAGE 1", null, "ESSA NOTAM MESSAGE 1\n\nESSA NOTAM MESSAGE 2" }
            )]
        [InlineData(
            "data/api-responses/esmx-essi-esmi.json",
            new[] { "ESMX", "ESSI", "ESMI" },
            new[] { "ESMX NOTAM MESSAGE 1\n\nESMX NOTAM MESSAGE 2", null, null }
            )]
        public void Parse(string responseJsonPath, string[] airportIcaos, string[] expectedNotams)
        {
            // Arrange
            var responseJson = File.ReadAllText(responseJsonPath);

            // Act
            var parser = new ResponseParser();
            IReadOnlyDictionary<string, string> result = parser.ParseJson(responseJson);

            // Assert
            for (int i = 0; i < expectedNotams.Length; i++)
            {
                string airportIcao = airportIcaos[i];
                string expectedNotam = expectedNotams[i];

                if (expectedNotam != null)
                {
                    Assert.True(result.ContainsKey(airportIcao));
                    Assert.Equal(expectedNotam, result[airportIcao]);
                }
            }
        }
    }
}
