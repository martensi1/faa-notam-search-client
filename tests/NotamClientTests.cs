using Moq;
using System;
using System.Collections.Generic;
using Xunit;

#nullable enable

namespace PilotAppLib.Clients.NotamSearch.Tests
{
    public class NotamClientTests
    {
        private readonly Mock<IApiClient> _apiMock;
        private readonly NotamSearchClient _client;


        public NotamClientTests()
        {
            _apiMock = new Mock<IApiClient>();
            _client = new NotamSearchClient(_apiMock.Object);
        }


        [Fact]
        public void DefaultConstructor()
        {
            new NotamSearchClient();
        }

        [Fact]
        public void Dispose()
        {
            // Arrange
            _apiMock.Setup(p => p.Dispose());

            // Act
            _client.Dispose();

            // Assert
            _apiMock.Verify(p => p.Dispose(), Times.Once);
        }

        [Theory]
        [InlineData("ESGJ", "AERODROME OPERATION HOURS OF SERVICE CHANGE TO...")]
        [InlineData("ESSI", "NIL")]
        [InlineData("ESGG", "REF AIP AD 2 ESGG 2.20 LOCAL TRAFFIC REGULATIONS, PARA 7.1 C...")]
        public void FetchNotam(string airportIcao, string airportNotam)
        {
            // Arrange
            var fetchResult = CreateNotamDictionary(new[] { airportIcao }, new[] { airportNotam });

            _apiMock.Setup(p => p.GetNotams(
                It.IsAny<string[]>()
            )).Returns(fetchResult);

            // Act
            string result = _client.FetchNotam(airportIcao);

            // Assert
            Assert.Equal(airportNotam, result);
            _apiMock.Verify(p => p.GetNotams(new[] { airportIcao }), Times.Once);
        }

        [Theory]
        [InlineData(new[] { "ESGJ", "ESSA" }, new[] { "NIL", "REF AIP AD 2 ESGG 2.20 LOCAL TRAFFIC REGULATIONS, PARA 7.1 C..." })]
        [InlineData(new[] { "ESGJ" }, new[] { "AERODROME OPERATION HOURS OF SERVICE CHANGE TO..." })]
        public void FetchNotams(string[] airportIcaos, string[] airportNotams)
        {
            // Arrange
            var notamData = CreateNotamDictionary(airportIcaos, airportNotams);

            _apiMock.Setup(p => p.GetNotams(
                It.IsAny<string[]>()
            )).Returns(notamData);

            // Act
            IReadOnlyDictionary<string, string> result = _client.FetchNotam(airportIcaos);

            // Assert
            Assert.Equal(notamData, result);
            _apiMock.Verify(p => p.GetNotams(airportIcaos), Times.Once);
        }

        [Fact]
        public void FetchNotamArgumentNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _client.FetchNotam((string?)null));
            Assert.Equal("airport", ex.ParamName);
        }

        [Fact]
        public void FetchNotamsArgumentNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => _client.FetchNotam((string[]?)null));
            Assert.Equal("airports", ex.ParamName);
        }

        [Fact]
        public void FetchNotamsLengthZero()
        {
            var ex = Assert.Throws<ArgumentException>(() => _client.FetchNotam(new string[0]));
            Assert.Equal("airports", ex.ParamName);
        }


        private IReadOnlyDictionary<string, string> CreateNotamDictionary(string[] airportIcaos, string[] airportNotams)
        {
            if (airportIcaos.Length != airportNotams.Length)
                throw new ArgumentException("Array lengths does not match");


            var result = new Dictionary<string, string>();

            for (int i = 0; i < airportIcaos.Length; i++)
            {
                result.Add(airportIcaos[i], airportNotams[i]);
            }

            return result;
        }
    }
}
