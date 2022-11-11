using System;
using System.Collections.Generic;

namespace PilotAppLib.Clients.NotamSearch
{
    interface IApiClient : IDisposable
    {
        public IReadOnlyDictionary<string, List<NotamRecord>> GetNotams(string[] airports);
    }


    sealed class ApiClient : IApiClient
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


        public IReadOnlyDictionary<string, List<NotamRecord>> GetNotams(string[] airportIcaos)
        {
            NotamRecordBatch result = FetchAllNotams(airportIcaos);
            result.SortAll();

            return result.Records;
        }

        
        private NotamRecordBatch FetchAllNotams(string[] airportIcaos)
        {
            NotamRecordBatch result = CreateStartBatch(airportIcaos);
            uint nextOffset = 0;
            
            while (true)
            {
                NotamRecordBatch parseResult = FetchNextBatch(airportIcaos, nextOffset);
                result.Append(parseResult);

                if (parseResult.LastBatch)
                {
                    
                    break;
                }
                else
                {
                    nextOffset = parseResult.EndCount;
                    continue;
                }
            }

            return result;
        }

        private NotamRecordBatch FetchNextBatch(string[] airportIcaos, uint offset)
        {
            string postEndpoint = _endpointBuilder.BuildHttpEndpoint(airportIcaos, offset);
            string responseJson = _httpGateway.SendPost(postEndpoint);

            NotamRecordBatch recordBatch = _responseParser.ParseJson(responseJson);
            return recordBatch;
        }

        private NotamRecordBatch CreateStartBatch(string[] airportIcaos)
        {
            var startBatch = new NotamRecordBatch();

            foreach (string airportIcao in airportIcaos)
            {
                startBatch.Records.Add(airportIcao, new List<NotamRecord>());
            }

            return startBatch;
        }
    }
}