using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;
using System.Text;

namespace webapi.Hubs
{
    public class AsyncExtractorHub : Hub
    {
        /// <summary>
        /// This is the stream that the frontend can access using signalr hubs and this will call the stream writer that will return each character until it is completed or resulted to error.
        /// </summary>
        /// <param name="inputTxt">Frontend can pass their input that they want to convert to base64 format</param>
        /// <param name="cancellationToken">Frontend may dispose their subscription to the stream using this cancellation token</param>
        /// <returns></returns>
        public ChannelReader<char> Extract(
        string inputTxt,
        CancellationToken cancellationToken)
        {
            var channel = Channel.CreateUnbounded<char>();
            _ = WriteItemsAsync(channel.Writer, inputTxt, cancellationToken);
            return channel.Reader;
        }

        /// <summary>
        /// This will encode the inputTxt and will convert into its base64 format.
        /// Each character of the base64 will be pass to the steam one by one with random delays between 1-5 secs.
        /// </summary>
        /// <param name="writer">This will be the medium of the frontend to fetch the result realtime</param>
        /// <param name="inputTxt">The text that will be converted to base64 format</param>
        /// <param name="cancellationToken">Access to cancel the process of converting and writing</param>
        /// <returns></returns>
        private async Task WriteItemsAsync(
            ChannelWriter<char> writer,
            string inputTxt,
            CancellationToken cancellationToken)
        {
            Exception localException = null;
            try
            {
                Random rnd = new Random();                

                var output = Convert.ToBase64String(Encoding.UTF8.GetBytes(inputTxt));

                foreach (var c in output)
                {
                    int delay = (rnd.Next(1, 5)) * 1000;
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
