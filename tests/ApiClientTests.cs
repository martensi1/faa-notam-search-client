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
            new string[] { "essa-essl-ekrn-1.json", "essa-essl-ekrn-2.json" },
            new uint[] { 0, 30 },
            new string[] { "ESSA", "ESSL", "EKRN", "ESMX" },
            new uint[] { 10, 12, 13, 0 }
            )]
        [InlineData(
            new string[] { "esmx-essi-esmi-1.json", "esmx-essi-esmi-2.json" },
            new uint[] { 0, 2 },
            new string[] { "ESMX", "ESSI", "ESMI" },
            new uint[] { 4, 0, 0, 0 }
            )]
        [InlineData(
            new string[] { "esgj-esgg-essa-1.json",  },
            new uint[] { 0 },
            new string[] { "ESGJ", "ESGG", "ESSA", "ESGR" },
            new uint[] { 1, 0, 2, 0 }
            )]
        public void GetNotams(string[] responseFileNames, uint[] offsets, string[] icaos, uint[] notamCount)
        {
            // Arrange
            for (int i = 0; i < responseFileNames.Length; i++)
            {
                SetupHttpMock(icaos, offsets[i], responseFileNames[i]);
            }

            // Act
            var result = _client.GetNotams(icaos);

            // Assert
            Array.ForEach(offsets, offset => VerifyHttpMock(icaos, offset));

            Assert.Equal(icaos.Length, result.Count);
            Array.ForEach(icaos, a => Assert.True(result.ContainsKey(a)));

            for (int i = 0; i < icaos.Length; i++)
            {
                var airportNotams = result[icaos[i]];
                
                Assert.Equal(notamCount[i], (uint)airportNotams.Count);
                Assert.True(IsListSorted(airportNotams));
            }
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


        private void SetupHttpMock(string[] airports, uint offset, string responseFileName)
        {
            var responseJson = File.ReadAllText("data/api-responses/" + responseFileName);
            string httpEndpoint = (new EndpointBuilder()).BuildHttpEndpoint(airports, offset);

            _httpGatewayMock.Setup(p => p.SendPost(httpEndpoint))
                .Returns(responseJson);
        }

        private void VerifyHttpMock(string[] airports, uint offset)
        {
            string expectedEndpoint = (new EndpointBuilder()).BuildHttpEndpoint(airports, offset);
            _httpGatewayMock.Verify(p => p.SendPost(expectedEndpoint), Times.Once);
        }

        private bool IsListSorted(List<NotamRecord> notams)
        {
            if (notams.Count <= 1)
                return true;

            NotamRecord lastElement = notams[0];

            return notams.Skip(1).All(nextElement =>
            {
                bool result = nextElement.CompareTo(lastElement) > 0;
                lastElement = nextElement;

                return result;
            });
        }
    }
}
