using System.IO;
using System.Threading.Tasks;

namespace Microblink.Extensions
{
    public static class StreamExtension
    {
        public static long GetStreamLength(this Stream stream)
        {
            return stream.GetStreamLengthAsync().GetAwaiter().GetResult();
        }

        public async static Task<long> GetStreamLengthAsync(this Stream stream)
        {
            if (stream.IsNull())
                return 0;

            long originalPosition = 0;
            long totalBytesRead = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(readBuffer, 0, 4096)) > 0)
                {
                    totalBytesRead += bytesRead;
                }

            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }

            return totalBytesRead;
        }

        /// <summary>
        /// Coding sugar for null check
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool IsNull(this Stream stream)
        {
            return stream == null;
        }
    }
}
