using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PilotAppLib.Clients.NotamSearch.Tests
{
    public class NotamRecordTests
    {
        [Fact]
        public void Constructor()
        {
            // Act and assert
            new NotamRecord("ICAO", "A1235/22", "NOTAM MESSAGE");
        }

        [Theory]
        [InlineData(null, "A1235/22", "NOTAM MESSAGE")]
        [InlineData("ICAO", null, "NOTAM MESSAGE")]
        [InlineData("ICAO", "A1235/22", null)]
        public void ConstructorNullArguments(string icaoCode, string notamNumber, string notamMessage)
        {
            // Act and assert
            var ex = Assert.Throws<ArgumentNullException>(() => new NotamRecord(icaoCode, notamNumber, notamMessage));
        }

        [Theory]
        [InlineData("A1235/22/1")]
        [InlineData("11234/22")]
        [InlineData("A123C/22")]
        [InlineData("A1234/2A")]
        [InlineData("A123422")]
        public void ConstructorInvalidIcaoCode(string icaoCode)
        {
            // Act and assert
            var ex = Assert.Throws<ArgumentException>(() => new NotamRecord(icaoCode, "A1235/22", ""));
            Assert.StartsWith("Invalid ICAO code", ex.Message);
            Assert.Equal("icaoCode", ex.ParamName);
        }

        [Theory]
        [InlineData("A1235/22/1")]
        [InlineData("11234/22")]
        [InlineData("A123C/22")]
        [InlineData("A1234/2A")]
        [InlineData("A123422")]
        public void ConstructorInvalidNotamNumber(string notamNumber)
        {
            // Act and assert
            var ex = Assert.Throws<ArgumentException>(() => new NotamRecord("ICAO", notamNumber, ""));
            Assert.StartsWith("Invalid NOTAM number", ex.Message);
            Assert.Equal("notamNumber", ex.ParamName);
        }

        [Theory]
        [InlineData("A1234/22", "A1235/21", -1)]
        [InlineData("A1235/22", "A1236/22", 1)]
        [InlineData("A1233/22", "A1235/21", -1)]
        [InlineData("A1234/22", "A1234/22", 0)]
        [InlineData("A1234/22", "B1235/21", -1)]
        [InlineData("A1234/22", "B1235/22", 1)]
        public void CompareTo(string number1, string number2, int expectedValue)
        {
            // Arrange
            var record1 = CreateNotamRecordWithNumber(number1);
            var record2 = CreateNotamRecordWithNumber(number2);

            // Act
            int actualValue = Math.Sign(record1.CompareTo(record2));

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void CompareToNull()
        {
            // Arrange
            var record1 = CreateNotamRecordWithNumber("A1234/22");

            // Act
            int actualValue = record1.CompareTo(null);

            // Assert
            Assert.Equal(1, actualValue);
        }

        [Fact]
        public void Sorting()
        {
            // Arrange
            var listOfRecords = new List<NotamRecord>() {
                CreateNotamRecordWithNumber("A1234/22"),
                CreateNotamRecordWithNumber("B1234/22"),
                CreateNotamRecordWithNumber("A1235/21"),
                CreateNotamRecordWithNumber("A1236/22"),
            };

            var expectedSortedList = new List<NotamRecord>() {
                CreateNotamRecordWithNumber("B1234/22"),
                CreateNotamRecordWithNumber("A1236/22"),
                CreateNotamRecordWithNumber("A1234/22"),
                CreateNotamRecordWithNumber("A1235/21"),
            };

            // Act
            listOfRecords.Sort();

            // Assert
            Assert.True(expectedSortedList.SequenceEqual(listOfRecords));
        }

        [Fact]
        public void ConvertToString()
        {
            // Arrange
            NotamRecord record = new NotamRecord("ESGJ", "B1234/22", "This is a test");

            // Act and assert
            Assert.Equal("B1234/22: This is a test", record.ToString());
        }

        [Theory]
        [InlineData("ESGJ", "A1235/21", "This is a test", "ESGJ", "A1235/21", "This is a test", true)]
        [InlineData("ESGJ", "A1235/21", "This is a test", "ESGJ", "A1234/21", "This is a test", false)]
        [InlineData("ESGJ", "A1235/21", "This is a test!", "ESGJ", "A1235/21", "This is a test", false)]
        [InlineData("ESGJ", "A1235/21", "This is a test", "ESSA", "A1235/21", "This is a test", false)]
        public void EqualsAndHashcode(
            string icao1, string number1, string message1,
            string icao2, string number2, string message2,
            bool expectedValue
            )
        {
            // Arrange
            var record1 = new NotamRecord(icao1, number1, message1);
            var record2 = new NotamRecord(icao2, number2, message2);

            // Act and assert
            Assert.Equal(expectedValue, record1.Equals(record2));
            Assert.Equal(expectedValue, record1.GetHashCode() == record2.GetHashCode());
        }

        [Fact]
        public void EqualsObject()
        {
            // Arrange
            NotamRecord record = new NotamRecord("ESGJ", "A1235/21", "This is a test");

            object recordEqual = new NotamRecord("ESGJ", "A1235/21", "This is a test");
            object recordNotEqual = new NotamRecord("ESGJ", "A1234/21", "This is a test");
            object nullValue = null;
            object stringValue = "This is a string";

            // Act and assert
            Assert.True(record.Equals(recordEqual));
            Assert.False(record.Equals(recordNotEqual));
            Assert.False(record.Equals(nullValue));
            Assert.False(record.Equals(stringValue));
        }
        
        [Fact]
        public void EqualsNull()
        {
            // Arrange
            var record1 = new NotamRecord("ESGJ", "A1235/21", "This is a test");

            // Act and assert
            Assert.False(record1.Equals(null));
        }


        private NotamRecord CreateNotamRecordWithNumber(string notamNumber)
        {
            return new NotamRecord("ICAO", notamNumber, "");
        }
    }
}
