using Moq;
using System.Dynamic;
using Xunit;
using webapi;
using webapi.Hubs;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Channels;
using System.ServiceModel;
using Xunit.Abstractions;
using Xunit.Sdk;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

namespace webapitest.Hubs
{
    public class AsyncExtractorHubTest
    {
        private readonly ITestOutputHelper _outputHelper;

        public AsyncExtractorHubTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Theory]
        [InlineData("SampleData", "U2FtcGxlRGF0YQ==")]
        public async void SendText_Extractor_ReturnBase64Format(string input, string expected)
        {
            var token = new CancellationTokenSource().Token;
            var hub = new AsyncExtractorHub();
            var output = new StringBuilder();
            var stream = hub.Extract(input, token);

            while (await stream.WaitToReadAsync())
            {
                while (stream.TryRead(out var data))
                {
                    output.Append(data);
                }
            }
            
            Assert.True(output.ToString() == expected);
        }
    }
}