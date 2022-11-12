using System;
using System.Collections.Generic;

namespace PilotAppLib.Clients.NotamSearch
{
    sealed class NotamRecordBatch
    {
        public uint StartCount { get; set; }

        public uint EndCount { get; set; }

        public bool LastBatch { get; set; }

        public Dictionary<string, List<NotamRecord>> Records { get; set; }

            
        public NotamRecordBatch()
        {
            StartCount = 0;
            EndCount = 0;
            LastBatch = false;
            Records = new Dictionary<string, List<NotamRecord>>();
        }

        public void Append(NotamRecordBatch other)
        {
            if (other.StartCount != (EndCount + 1))
            {
                throw new ArgumentException("Start/end count mismatch, batches not in sequence", nameof(other));
            }

            EndCount = other.EndCount;
            LastBatch = other.LastBatch;

            foreach (var (icao, notams) in other.Records)
            {
                if (Records.TryGetValue(icao, out var existingNotams))
                {
                    existingNotams.AddRange(notams);
                }
                else
                {
                    Records.Add(icao, notams);
                }
            }
        }

        public void SortAll()
        {
            foreach (var notams in Records.Values)
            {
                notams.Sort();
            }
        }
    };
}
