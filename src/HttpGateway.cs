using PilotAppLib.Http;
using System;

namespace PilotAppLib.Clients.NotamSearch
{
    interface IHttpGateway : IDisposable
    {
        public string SendPost(string endpoint);
    }

    class HttpGateway : IHttpGateway
    {
        private readonly SimpleHttp _httpClient;


        public HttpGateway()
        {
            _httpClient = new SimpleHttp();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }


        public string SendPost(string endpoint)
        {
            SimpleHttp.HttpResponse response = _httpClient.Post(endpoint, "application/json");
            return response.AsString();
        }
    }
}
