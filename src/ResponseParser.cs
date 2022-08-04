using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PilotAppLib.Clients.NotamSearch
{
    interface IResponseParser
    {
        public IReadOnlyDictionary<string, string> ParseJson(string json);
    }

    class ResponseParser : IResponseParser
    {
        public IReadOnlyDictionary<string, string> ParseJson(string json)
        {
            JsonDocument document = JsonDocument.Parse(json);
            JsonElement list = document.RootElement.GetProperty("notamList");

            var items = list.EnumerateArray()
                .Select(it => JsonSerializer.Deserialize<ResponseItem>(it.GetRawText()))
                .ToList();

            return IterateItems(items);
        }


        private IReadOnlyDictionary<string, string> IterateItems(IReadOnlyList<ResponseItem> items)
        {
            Dictionary<string, string> reports = new Dictionary<string, string>();

            foreach (ResponseItem item in items)
            {
                string airport = item.IcaoCode;

                if (!reports.ContainsKey(airport))
                {
                    reports.Add(airport, string.Empty);
                }
                else
                {
                    reports[airport] += 
                        "\n" +
                        "\n";
                }

                reports[airport] += item.Message;
            }

            return reports;
        }
    }
}
