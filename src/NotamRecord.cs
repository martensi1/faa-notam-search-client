using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace PilotAppLib.Clients.NotamSearch
{
    /// <summary>
    /// NOTAM record object, respresents a single NOTAM
    /// </summary>
    [DebuggerDisplay("(IcaoCode={IcaoCode}, NotamNumber={NotamNumber})")]
    public sealed class NotamRecord 
        : IEquatable<NotamRecord>, IComparable<NotamRecord>
    {
        /// <summary>
        /// ICAO indicator of the aerodrome
        /// </summary>
        public string IcaoCode { get; private set; }

        /// <summary>
        /// NOTAM number (unique identifier)
        /// </summary>
        public string NotamNumber { get; private set; }

        /// <summary>
        /// NOTAM message/text
        /// </summary>
        public string Message { get; private set; }


        /// <summary>Initializes a new instance of the <see cref="NotamRecord" /> class</summary>
        /// <exception cref="ArgumentNullException">One of the arguments is a null value</exception>
        /// <exception cref="ArgumentException">Invalid ICAO code or NOTAM number</exception>
        internal NotamRecord(string icaoCode, string notamNumber, string message)
        {
            ThrowIfNullArgument(icaoCode, nameof(icaoCode));
            ThrowIfNullArgument(notamNumber, nameof(notamNumber));
            ThrowIfNullArgument(message, nameof(message));

            ThrowIfInvalidIcaoCode(icaoCode, nameof(icaoCode));
            ThrowIfInvalidNotamNumber(notamNumber, nameof(notamNumber));

            IcaoCode = icaoCode;
            NotamNumber = notamNumber;
            Message = message;
        }

        
        /// <summary>Returns a string that represents the current object</summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString()
        {
            return $"{NotamNumber}: {Message}";
        }

        /// <summary>Determines whether the specified object is equal to the current object</summary>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/></returns>
        public override bool Equals(object other)
        {
            return other is NotamRecord value &&
                Equals(value);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type</summary>
        /// <returns><see langword="true"/> if the current object is equal to the other parameter; otherwise, <see langword="false"/></returns>
        public bool Equals(NotamRecord other)
        {
            if (other == null) 
                return false;

            return
                IcaoCode == other.IcaoCode &&
                NotamNumber == other.NotamNumber &&
                Message == other.Message;
        }

        /// <summary>Serves as the default hash function</summary>
        /// <returns>A hash code for the current object</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(
                IcaoCode,
                NotamNumber,
                Message
                );
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Less than zero: This instance precedes other in the sort order
        /// Zero: This instance occurs in the same position in the sort order as other
        /// Greater than zero: This instance follows the other in the sort order
        /// </returns>
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

            string seriesAndNumber = parts[0];
            string year = parts[1];

            char series = seriesAndNumber.ToCharArray().First();
            string number = seriesAndNumber.Substring(1);

            string compareValue = $"{year}{(int)series}{number}";
            return int.Parse(compareValue);
        }

        private void ThrowIfNullArgument(object argument, string paramName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        private void ThrowIfInvalidIcaoCode(string icaoCode, string paramName)
        {
            if (!Regex.IsMatch(icaoCode, @"^[A-Z]{4}$"))
            {
                throw new ArgumentException($"Invalid ICAO code {icaoCode}", paramName);
            }
        }
        
        private void ThrowIfInvalidNotamNumber(string notamNumber, string paramName)
        {
            if (!Regex.IsMatch(notamNumber, @"^[A-Z]\d{4}/\d{2}$"))
            {
                throw new ArgumentException($"Invalid NOTAM number {notamNumber}", paramName);
            }
        }
    }
}
