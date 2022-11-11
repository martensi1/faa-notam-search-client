using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PilotAppLib.Clients.NotamSearch.Tests
{
    public class NotamRecordTests
    {
        [Theory]
        [InlineData("A1234/22", "A1235/21", -1)]
        [InlineData("A1235/22", "A1236/22", 1)]
        [InlineData("A1233/22", "A1235/21", -1)]
        [InlineData("A1234/22", "A1234/22", 0)]
        [InlineData("A1234/22", "B1235/21", -1)]
        [InlineData("A1234/22", "B1235/22", 1)]
        public void Compare(string number1, string number2, int expectedValue)
        {
            var item1 = CreateReponseItem(number1);
            var item2 = CreateReponseItem(number2);

            int actualValue = Math.Sign(item1.CompareTo(item2));
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Sorting()
        {
            var listOfItems = new List<NotamRecord>() {
                CreateReponseItem("A1234/22"),
                CreateReponseItem("B1234/22"),
                CreateReponseItem("A1235/21"),
                CreateReponseItem("A1236/22"),
            };

            var expectedSortedList = new List<NotamRecord>() {
                CreateReponseItem("B1234/22"),
                CreateReponseItem("A1236/22"),
                CreateReponseItem("A1234/22"),
                CreateReponseItem("A1235/21"),
            };

            listOfItems.Sort();
            Assert.True(expectedSortedList.SequenceEqual(listOfItems));
        }


        private NotamRecord CreateReponseItem(string notamNumber)
        {
            return new NotamRecord {
                NotamNumber = notamNumber
            };
        }
    }
}
