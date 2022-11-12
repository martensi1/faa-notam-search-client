using PilotAppLib.Clients.NotamSearch;
using System;

namespace PilotAppLib.Sandbox
{
    public class Sandbox
    {
        static void Main()
        {
            using (var client = new NotamSearchClient())
            {
                var airportNotams = client.FetchNotams(new string[] { "ESSA", "ESSL", "EKRN", "ESMX" });
                
                foreach (var notams in airportNotams)
                {
                    foreach (var notam in notams.Value)
                    {
                        Console.WriteLine(notam);
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
