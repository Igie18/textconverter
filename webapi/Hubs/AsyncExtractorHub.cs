using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Text;

namespace webapi.Hubs
{
    public class AsyncExtractorHub : Hub
    {
        public ChannelReader<char> Extract(
        string inputTxt,
        CancellationToken cancellationToken)
        {
            var channel = Channel.CreateUnbounded<char>();
            _ = WriteItemsAsync(channel.Writer, inputTxt, cancellationToken);
            return channel.Reader;
        }

        private async Task WriteItemsAsync(
            ChannelWriter<char> writer,
            string inputTxt,
            CancellationToken cancellationToken)
        {
            Exception localException = null;
            try
            {
                Random rnd = new Random();
                int delay = (rnd.Next(1, 5)) * 1000;

                var output = Convert.ToBase64String(Encoding.UTF8.GetBytes(inputTxt));

                foreach (var c in output)
                {
                    await writer.WriteAsync(c, cancellationToken);
                    await Task.Delay(delay, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                localException = ex;
            }
            finally
            {
                writer.Complete(localException);
            }
        }
    }
}
