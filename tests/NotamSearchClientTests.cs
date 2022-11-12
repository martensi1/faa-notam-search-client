using Moq;
using System;
using System.Collections.Generic;
using Xunit;

#nullable enable

namespace PilotAppLib.Clients.NotamSearch.Tests
{
    public class NotamSearchClientTests
    {
        private readonly Mock<IApiClient> _apiMock;
        private readonly NotamSearchClient _client;


        public NotamSearchClientTests()
        {
            _apiMock = new Mock<IApiClient>();
            _client = new NotamSearchClient(_apiMock.Object);
        }


        [Fact]
        public void DefaultConstructor()
        {
            // Act and assert
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
        [InlineData("ESGJ")]
        [InlineData("ESSI")]
        [InlineData("ESGG")]
        public void FetchNotams1(string airportIcao)
        {
            // Arrange
            var notamData = new Dictionary<string, List<NotamRecord>>() {
                { airportIcao, new List<NotamRecord>() }
            };

            _apiMock.Setup(p => p.GetNotams(
                It.IsAny<string[]>()
            )).Returns(notamData);
            
            // Act
            List<NotamRecord> result = _client.FetchNotams(airportIcao);

            // Assert
            Assert.Same(notamData[airportIcao], result);
            _apiMock.Verify(p => p.GetNotams(new[] { airportIcao }), Times.Once);
        }

        [Theory]
        [InlineData(new[] { "ESGJ", "ESSA" }, 0)]
        [InlineData(new[] { "ESGJ" }, 0)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters", Justification = "Bug, does not compile with only string array as argument")]
        public void FetchNotams2(string[] airportIcaos, int _)
        {
            // Arrange
            var notamData = new Dictionary<string, List<NotamRecord>>();


            _apiMock.Setup(p => p.GetNotams(
                It.IsAny<string[]>()
            )).Returns(notamData);
            
            // Act
            var result = _client.FetchNotams(airportIcaos);

            // Assert
            Assert.Same(notamData, result);
            _apiMock.Verify(p => p.GetNotams(airportIcaos), Times.Once);
        }

        [Fact]
        public void FetchNotams1ArgumentNull()
        {
            // Act and assert
            var ex = Assert.Throws<ArgumentNullException>(() => _client.FetchNotams((string?)null));
            Assert.Equal("airport", ex.ParamName);
        }

        [Fact]
        public void FetchNotams2ArgumentNull()
        {
            // Act and assert
            var ex = Assert.Throws<ArgumentNullException>(() => _client.FetchNotams((string[]?)null));
            Assert.Equal("airports", ex.ParamName);
        }

        [Fact]
        public void FetchNotamsLengthZero()
        {
            // Act and assert
            var ex = Assert.Throws<ArgumentException>(() => _client.FetchNotams(new string[0]));
            Assert.Equal("airports", ex.ParamName);
        }
    }
}
