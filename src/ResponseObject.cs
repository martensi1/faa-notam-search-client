using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace PilotAppLib.Clients.NotamSearch
{
    [ExcludeFromCodeCoverage]
    sealed class ResponseObject 
    {
        [JsonPropertyName("startRecordCount")]
        public uint StartRecordCount { get; set; }

        [JsonPropertyName("endRecordCount")]
        public uint EndRecordCount { get; set; }

        [JsonPropertyName("totalNotamCount")]
        public uint TotalRecordCount { get; set; }


        public sealed class NotamObject
        {
            [JsonPropertyName("facilityDesignator")]
            public string IcaoCode { get; set; }

            [JsonPropertyName("notamNumber")]
            public string NotamNumber { get; set; }

            [JsonPropertyName("icaoMessage")]
            public string Message { get; set; }
        }

        [JsonPropertyName("notamList")]
        public List<NotamObject> Objects { get; set; }
    }
}
