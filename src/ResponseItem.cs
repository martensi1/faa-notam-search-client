using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace PilotAppLib.Clients.NotamSearch
{
    [ExcludeFromCodeCoverage]
    sealed class ResponseItem
    {
        [JsonPropertyName("facilityDesignator")]
        public string IcaoCode { get; set; }

        [JsonPropertyName("icaoMessage")]
        public string Message { get; set; }
    }
}
