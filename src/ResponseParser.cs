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
            var records = responseObject.Records;
            var result = new Dictionary<string, List<NotamRecord>>();

            foreach (NotamRecord record in records)
            {
                string icao = record.IcaoCode;

                if (!result.ContainsKey(icao))
                {
                    result.Add(icao, new List<NotamRecord>());
                }

                record.Message = StripControlCharacters(record.Message);
                result[icao].Add(record);
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
