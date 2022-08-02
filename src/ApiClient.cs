using System;
using System.Collections.Generic;

namespace PilotAppLib.Clients.NotamSearch
{
    interface IApiClient : IDisposable
    {
        public IReadOnlyDictionary<string, string> GetNotams(string[] airports);
    }


    class ApiClient : IApiClient
    {
        private readonly IEndpointBuilder _endpointBuilder;
        private readonly IHttpGateway _httpGateway;
        private readonly IResponseParser _responseParser;


        public ApiClient(
            IEndpointBuilder endpointBuilder,
            IHttpGateway httpGateway,
            IResponseParser responseParser
            )
        {
            _endpointBuilder = endpointBuilder;
            _httpGateway = httpGateway;
            _responseParser = responseParser;
        }

        public void Dispose()
        {
            _httpGateway.Dispose();
        }


        public IReadOnlyDictionary<string, string> GetNotams(string[] airports)
        {
            string endpoint = _endpointBuilder.BuildHttpEndpoint(airports);
            string text = _httpGateway.SendPost(endpoint);

            return _responseParser.ParseJson(text);
        }
    }
}