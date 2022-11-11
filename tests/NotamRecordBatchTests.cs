using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PilotAppLib.Clients.NotamSearch.Tests
{
    public class NotamRecordBatchTests
    {
        [Fact]
        public void DefaultConstructor()
        {
            // Act
            var parseResult = new NotamRecordBatch();
            
            // Assert
            Assert.Equal((uint)0, parseResult.StartCount);
            Assert.Equal((uint)0, parseResult.EndCount);
            Assert.False(parseResult.LastBatch);
            Assert.Empty(parseResult.Records);
        }

        [Theory]
        [InlineData(0, 1, 2, 2)]
        [InlineData(0, 2, 3, 4)]
        [InlineData(8, 16, 17, 24)]
        [InlineData(0, 30, 31, 60)]
        public void AppendCheckStartEndCount(
            uint firstStartCount, uint firstEndCount,
            uint secondStartCount, uint secondLastCount
            )
        {
            // Arrange
            var first = new NotamRecordBatch() {
                StartCount = firstStartCount,
                EndCount = firstEndCount,
                Records = new Dictionary<string, List<NotamRecord>>()
            };

            var second = new NotamRecordBatch() {
                StartCount = secondStartCount,
                EndCount = secondLastCount,
                Records = new Dictionary<string, List<NotamRecord>>()
            };

            // Act
            first.Append(second);

            // Assert
            Assert.Equal(firstStartCount, first.StartCount);
            Assert.Equal(secondLastCount, first.EndCount);
        }
        
        /*
        [Theory]
        [InlineData(0, 1, 1, 2)]
        [InlineData(0, 2, 2, 4)]
        [InlineData(8, 16, 16, 24)]
        [InlineData(0, 30, 30, 60)]
        public void AppendCheckRecords(
            uint fistStartCount, uint firstNextOffset,
            uint secondOffset, uint secondNextOffset
            )
        {
            // Arrange
            var first = new NotamRecordBatch() {
                StartCount = 0,
                EndCount = 2,
                Records = new Dictionary<string, List<NotamRecord>>() {
                    { 
                        "ESGJ",
                        new List<NotamRecord>() {
                            CreateRecord("ESGJ", "A0001/22", "NOTAM MESSAGE"),
                            CreateRecord("ESGJ", "A0001/22", "NOTAM MESSAGE"),
                        }
                    },
                     {
                        "ESSA",
                        new List<NotamRecord>() {
                            
                        }
                    }
                }
            };

            var second = new NotamRecordBatch() {
                StartCount = 2,
                EndCount = 4,
                Records = new Dictionary<string, List<NotamRecord>>()
            };

            // Act
            first.Append(second);

            // Assert
            //Assert.Equal(firstOffset, first.Offset);
            //Assert.Equal(secondNextOffset, first.NextOffset);
        }*/

        [Theory]
        [InlineData(2, 2)]
        [InlineData(2, 4)]
        [InlineData(30, 28)]
        [InlineData(40, 60)]
        public void AppendStartEndCountMismatch(uint firstEndCount, uint secondStartCount)
        {
            // Arrange
            var first = new NotamRecordBatch() {
                EndCount = firstEndCount
            };

            var second = new NotamRecordBatch() {
                StartCount = secondStartCount
            };

            // Act and assert
            var ex = Assert.Throws<ArgumentException>(() => first.Append(second));
            Assert.Equal("other", ex.ParamName);
        }

        
        private NotamRecord CreateRecord(string icao, string notamNumber, string message)
        {
            return new NotamRecord() {
                IcaoCode = icao,
                NotamNumber = notamNumber,
                Message = message
            };
        }
    }
}
