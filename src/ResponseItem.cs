using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;

namespace PilotAppLib.Clients.NotamSearch
{
    [ExcludeFromCodeCoverage]
    sealed class ResponseItem 
        : IEquatable<ResponseItem>, IComparable<ResponseItem>
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

            if (!(obj is ResponseItem other))
                return false;

            return Equals(other);
        }

        public bool Equals(ResponseItem other)
        {
            if (other == null) 
                return false;

            return (this.NotamNumber.Equals(other.NotamNumber));
        }

        public override int GetHashCode()
        {
            return NotamNumber.GetHashCode();
        }

        public int CompareTo(ResponseItem other)
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
