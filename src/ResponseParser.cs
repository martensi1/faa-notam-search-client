using System.Collections.Generic;
using System.Text.Json;

namespace PilotAppLib.Clients.NotamSearch
{
    interface IResponseParser
    {
        public NotamRecordBatch ParseJson(string json);
    }

    class ResponseParser : IResponseParser
    {
        public NotamRecordBatch ParseJson(string json)
        {
            ResponseObject responseObject 
                = JsonSerializer.Deserialize<ResponseObject>(json);

            return new NotamRecordBatch() {
                StartCount = GetStartCount(responseObject),
                EndCount = GetEndCount(responseObject),
                LastBatch = GetIsLastBatch(responseObject),
                Records = GetRecords(responseObject)
            };
        }


        private uint GetStartCount(ResponseObject responseObject)
        {
            return responseObject.StartRecordCount;
        }

        private uint GetEndCount(ResponseObject responseObject)
        {
            return responseObject.EndRecordCount;
        }
        private bool GetIsLastBatch(ResponseObject responseObject)
        {
            return (responseObject.EndRecordCount >= responseObject.TotalRecordCount);
        }

        private Dictionary<string, List<NotamRecord>> GetRecords(ResponseObject responseObject)
        {
            var notamObjects = responseObject.Objects;
            var result = new Dictionary<string, List<NotamRecord>>();

            foreach (var notamObject in notamObjects)
            {
                string icao = notamObject.IcaoCode;
                if (!result.ContainsKey(icao))
                {
                    result.Add(icao, new List<NotamRecord>());
                }

                notamObject.Message = StripControlCharacters(notamObject.Message);

                result[icao].Add(new NotamRecord(
                    notamObject.IcaoCode,
                    notamObject.NotamNumber,
                    notamObject.Message
                    ));
            }

            return result;
        }

        private string StripControlCharacters(string data)
        {
            return data
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty);
        }
    }
}
