using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace PilotAppLib.Clients.NotamSearch
{
    /// <summary>
    /// Client for retrieving NOTAMs from FAA's NOTAM Search service
    /// </summary>
    public class NotamSearchClient : IDisposable
    {
        private readonly IApiClient _apiClient;

        /// <summary>Initializes a new instance of the <see cref="NotamSearchClient" /> class</summary>
        public NotamSearchClient() : this(
            new ApiClient(
                new EndpointBuilder(),
                new HttpGateway(),
                new ResponseParser()
            ))
        {
        }

        internal NotamSearchClient(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        /// <summary>Releases the unmanaged resources and disposes of the managed resources used by <see cref="NotamSearchClient" /></summary>
        public void Dispose()
        {
            _apiClient.Dispose();
        }

        /// <summary>Retrieves the latest NOTAM for the specified airport</summary>
        /// <param name="airport">Airport to get NOTAM from (ICAO)</param>
        /// <returns>Airport NOTAM</returns>
        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout</exception>
        public string FetchNotam(string airport)
        {
            if (airport == null)
                throw new ArgumentNullException(nameof(airport));

            return _apiClient.GetNotams(new string[] { airport })
                .Values.First();
        }

        /// <summary>Retrieves the latest NOTAM for the specified list of airports</summary>
        /// <param name="airports">Airports to get NOTAM from (ICAO)</param>
        /// <returns>Airport NOTAMs as a dictionary (icao, notam)</returns>
        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout</exception>
        public IReadOnlyDictionary<string, string> FetchNotam(string[] airports)
        {
            if (airports == null)
                throw new ArgumentNullException(nameof(airports));

            if (airports.Length == 0)
                throw new ArgumentException("At least one airport must be specified", nameof(airports));

            return _apiClient.GetNotams(airports);
        }
    }
}