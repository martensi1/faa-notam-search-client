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
        
        [Fact]
        public void AppendCheckRecords()
        {
            // Arrange
            var first = new NotamRecordBatch() {
                StartCount = 1,
                EndCount = 2,
                Records = new Dictionary<string, List<NotamRecord>>() {
                    { 
                        "ESGJ",
                        new List<NotamRecord>() {
                            new NotamRecord("ESGJ", "A0001/22", "NOTAM MESSAGE"),
                            new NotamRecord("ESGJ", "A0002/22", "NOTAM MESSAGE"),
                        }
                    },
                    {
                        "ESSA",
                        new List<NotamRecord>() {
                            new NotamRecord("ESSA", "B0001/22", "NOTAM MESSAGE"),
                        }
                    }
                }
            };

            var second = new NotamRecordBatch() {
                StartCount = 3,
                EndCount = 4,
                Records = new Dictionary<string, List<NotamRecord>>() {
                     {
                        "ESSA",
                        new List<NotamRecord>() {
                            new NotamRecord("ESSA", "B0001/22", "NOTAM MESSAGE"),
                        }
                     },
                     {
                         "ESMX",
                         new List<NotamRecord>() {
                             new NotamRecord("ESMX", "C0001/22", "NOTAM MESSAGE"),
                             new NotamRecord("ESMX", "C0002/22", "NOTAM MESSAGE"),
                             new NotamRecord("ESMX", "C0003/22", "NOTAM MESSAGE")
                         }
                     }
                }
            };

            // Act
            first.Append(second);

            // Assert
            Assert.True(first.Records.ContainsKey("ESGJ"));
            Assert.True(first.Records.ContainsKey("ESSA"));
            Assert.True(first.Records.ContainsKey("ESMX"));

            Assert.Equal(2, first.Records["ESGJ"].Count);
            Assert.Equal(2, first.Records["ESSA"].Count);
            Assert.Equal(3, first.Records["ESMX"].Count);
        }

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
    }
}
