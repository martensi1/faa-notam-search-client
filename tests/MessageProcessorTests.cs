using System.Collections.Generic;
using System.IO;
using Xunit;

namespace PilotAppLib.Clients.NotamSearch.Tests
{
    public class MessageProcessorTests
    {
        [Fact]
        public void Process()
        {
            string result = MessageProcessor.Process("NOTAM \nMESS\nAGE");
            Assert.Equal("NOTAM MESSAGE", result);
        }
    }
}
