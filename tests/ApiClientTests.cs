using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace PilotAppLib.Clients.NotamSearch.Tests
{
    public class ApiClientTests
    {
        private readonly Mock<IHttpGateway> _httpGatewayMock;
        private readonly Mock<EndpointBuilder> _endpointBuilderMock;
        private readonly Mock<ResponseParser> _responseParserMock;
        private readonly ApiClient _client;


        public ApiClientTests()
        {
            _httpGatewayMock = new Mock<IHttpGateway>();
            _endpointBuilderMock = new Mock<EndpointBuilder>();
            _responseParserMock = new Mock<ResponseParser>();

            _client = new ApiClient(
                _endpointBuilderMock.Object,
                _httpGatewayMock.Object,
                _responseParserMock.Object
                );
        }


        [Theory]
        [InlineData(
            "data/api-responses/esgj-esgg-essa.json",
            new[] { "ESGJ", "ESGG", "ESSA" },
            new[] { "ESGJ", "ESSA" }
            )]
        [InlineData(
            "data/api-responses/esmx-essi-esmi.json",
            new[] { "ESMX", "ESSI", "ESMI" },
            new[] { "ESMX" }
            )]
        public void GetNotams(string responseJsonPath, string[] airportsToRequest, string[] airportsWithNotams)
        {
            // Arrange
            var responseJson = File.ReadAllText(responseJsonPath);

            _httpGatewayMock.Setup(p => p.SendPost(
                It.IsAny<string>()
            )).Returns(responseJson);

            // Act
            var result = _client.GetNotams(airportsToRequest);

            // Assert
            Assert.Equal(airportsWithNotams.Length, result.Count);
            Array.ForEach(airportsWithNotams, a => Assert.True(result.ContainsKey(a)));

            _httpGatewayMock.Verify(p => p.SendPost(GetEndpoint(airportsToRequest)), Times.Once);
        }

        [Fact]
        public void Dispose()
        {
            // Arrange
            _httpGatewayMock.Setup(p => p.Dispose());

            // Act
            _client.Dispose();

            // Assert
            _httpGatewayMock.Verify(p => p.Dispose(), Times.Once);
        }


        private string GetEndpoint(string[] airportIcaos)
        {
            return (new EndpointBuilder())
                .BuildHttpEndpoint(airportIcaos);
        }
    }
}
