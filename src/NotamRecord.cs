using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace PilotAppLib.Clients.NotamSearch
{
    public sealed class NotamRecord 
        : IEquatable<NotamRecord>, IComparable<NotamRecord>
    {
        [JsonPropertyName("facilityDesignator")]
        public string IcaoCode { get; set; }

        [JsonPropertyName("notamNumber")]
        public string NotamNumber { get; set; }

        [JsonPropertyName("icaoMessage")]
        public string Message { get; set; }


        public override string ToString()
        {
            return $"{NotamNumber} {Message}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is NotamRecord other))
                return false;

            return Equals(other);
        }

        public bool Equals(NotamRecord other)
        {
            if (other == null) 
                return false;

            return (this.NotamNumber.Equals(other.NotamNumber));
        }

        public override int GetHashCode()
        {
            return NotamNumber.GetHashCode();
        }

        public int CompareTo(NotamRecord other)
        {
            if (other == null)
            {
                // A null value means that this object is greater.
                return 1;
            }
            else
            {
                // Create numeric version of notam number and subtract, the most
                // recent notam number will have the highest numeric value
                return other.CreateNumericValue()
                      - this.CreateNumericValue();
            }
        }

        private int CreateNumericValue()
        {
            // Creates a comparable numeric number from the notam number. The most recent notam number
            // should have the highest numeric value
            //
            // A notam number can look like this: "A1234/21"
            // One letter to indicate the Series, a 4-digit NOTAM number followed by a stroke and two digits to indicate the year.
            //
            // Example of a correctly sorted notam numbers:
            // 1. 'A0001/22'
            // 2. 'B0001/21'
            // 3. 'A0002/21'
            // 4. 'A0001/21'
            string[] parts = NotamNumber.Split('/');

            if (parts.Length != 2)
                ThrowInvalidNotamNumber();

            string firstPart = parts[0];
            string yearPart = parts[1];

            if (firstPart.Length != 5 || yearPart.Length != 2)
                ThrowInvalidNotamNumber();

            char series = firstPart.ToCharArray().First();
            string number = firstPart.Substring(1);

            string compareValue = $"{yearPart}{(int)series}{number}";
            return int.Parse(compareValue);
        }

        private void ThrowInvalidNotamNumber()
        {
            throw new FormatException($"Invalid NOTAM number {NotamNumber}");
        }
    }
}
