using System.Collections.Generic;
using System.IO;
using Xunit;

namespace PilotAppLib.Clients.NotamSearch.Tests
{
    public class ResponseParserTests
    {
        private readonly IReadOnlyDictionary<string, NotamRecordBatch> ExpectedData 
            = new Dictionary<string, NotamRecordBatch>() {
                {
                    "data/api-responses/esgj-esgg-essa-1.json",
                    new NotamRecordBatch() {
                        StartCount = 1,
                        EndCount = 3,
                        LastBatch = true,
                        Records = new Dictionary<string, List<NotamRecord>>() {
                            {
                                "ESSA",
                                new List<NotamRecord>() { 
                                    new NotamRecord("ESSA", "A0575/22", "ESSA NOTAM MESSAGE 2"),
                                    new NotamRecord("ESSA", "A0668/22", "ESSA NOTAM MESSAGE 1")
                                }
                            },
                            { 
                                "ESGJ",
                                new List<NotamRecord>() {
                                    new NotamRecord("ESGJ", "B2547/22", "ESGJ NOTAM MESSAGE 1")
                                }
                            }
                        }
                    }
                },
                {
                    "data/api-responses/esmx-essi-esmi-1.json",
                    new NotamRecordBatch() {
                        StartCount = 1,
                        EndCount = 2,
                        LastBatch = false,
                        Records = new Dictionary<string, List<NotamRecord>>() {
                            {
                                "ESMX",
                                new List<NotamRecord>() {
                                    new NotamRecord("ESMX", "B2468/22", "ESMX NOTAM MESSAGE 2"),
                                    new NotamRecord("ESMX", "B2469/22", "ESMX NOTAM MESSAGE 1")
                                }
                            }
                        }
                    }
                },
                {
                    "data/api-responses/esmx-essi-esmi-2.json",
                    new NotamRecordBatch() {
                        StartCount = 3,
                        EndCount = 4,
                        LastBatch = true,
                        Records = new Dictionary<string, List<NotamRecord>>() {
                            {
                                "ESMX",
                                new List<NotamRecord>() {
                                    new NotamRecord("ESMX", "B2478/22", "ESMX NOTAM MESSAGE 4"),
                                    new NotamRecord("ESMX", "B2476/22", "ESMX NOTAM MESSAGE 3")
                                }
                            }
                        }
                    }
                }
        };
        
        [Theory]
        [InlineData("data/api-responses/esgj-esgg-essa-1.json")]
        [InlineData("data/api-responses/esmx-essi-esmi-1.json")]
        [InlineData("data/api-responses/esmx-essi-esmi-2.json")]
        public void ParseJson(string responseJsonPath)
        {
            // Arrange
            var responseJson = File.ReadAllText(responseJsonPath);

            // Act
            var parser = new ResponseParser();
            NotamRecordBatch result = parser.ParseJson(responseJson);

            // Assert
            var expectedData = ExpectedData[responseJsonPath];

            Assert.Equal(expectedData.StartCount, result.StartCount);
            Assert.Equal(expectedData.EndCount, result.EndCount);
            Assert.Equal(expectedData.LastBatch, result.LastBatch);

            CompareRecords(expectedData.Records, result.Records);
        }


        private void CompareRecords(
            IReadOnlyDictionary<string, List<NotamRecord>> expectedNotams,
            IReadOnlyDictionary<string, List<NotamRecord>> actualNotams
            )
        {
            foreach (string icao in expectedNotams.Keys)
            {
                Assert.True(actualNotams.ContainsKey(icao));

                var expectedNotamList = expectedNotams[icao];
                var actualNotamList = actualNotams[icao];
                
                Assert.Equal(expectedNotamList.Count, actualNotamList.Count);

                for (int i = 0; i < expectedNotamList.Count; i++)
                {
                    NotamRecord expectedRecord = expectedNotamList[i];
                    NotamRecord actualRecord = actualNotamList[i];

                    Assert.Equal(expectedRecord.IcaoCode, actualRecord.IcaoCode);
                    Assert.Equal(expectedRecord.Message, actualRecord.Message);
                    Assert.Equal(expectedRecord.NotamNumber, actualRecord.NotamNumber);
                }
            }
        }
    }
}
