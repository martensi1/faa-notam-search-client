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

        [JsonPropertyName("notamList")]
        public List<NotamRecord> Records { get; set; }
    }
}
