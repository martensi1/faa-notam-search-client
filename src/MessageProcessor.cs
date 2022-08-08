using System;

namespace PilotAppLib.Clients.NotamSearch
{
    static class MessageProcessor
    {
        public static string Process(string notamMessage)
        {
            return notamMessage
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty);
        }
    }
}