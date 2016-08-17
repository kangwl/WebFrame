using System.IO;
using System.Net;

namespace Test.Util.Common.ExtensionMethods
{
    /// <summary>
    ///     Web Extensions.
    /// </summary>
    public static class WebExtensions
    {
        /// <summary>
        ///     Downloads data from a url.
        /// </summary>
        /// <param name="url">Url to retrieve the data.</param>
        /// <returns>Byte array of data from the url.</returns>
        public static byte[] DownloadData(this string url)
        {
            var webRequest = WebRequest.Create(url);

            using (var webResponse = webRequest.GetResponse())
            {
                using (var stream = webResponse.GetResponseStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        var buffer = new byte[1024];

                        while (true)
                        {
                            var bytesRead = stream.Read(buffer, 0, buffer.Length);

                            if (bytesRead == 0)
                            {
                                break;
                            }
                            memoryStream.Write(buffer, 0, bytesRead);
                        }

                        memoryStream.Flush();

                        return memoryStream.ToArray();
                    }
                }
            }
        }
    }
}