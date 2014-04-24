using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Live
{
    internal static class HttpWebExtensionMethods
    {
        public static Task<Stream> GetRequestStreamAsync(this HttpWebRequest request)
        {
            var tcs = new TaskCompletionSource<Stream>();

            try
            {
                request.BeginGetRequestStream(iar =>
                    {
                        try
                        {
                            var response = request.EndGetRequestStream(iar);
                            tcs.SetResult(response);
                        }
                        catch (Exception exc)
                        {
                            tcs.SetException(exc);
                        }
                    }, null);
            }
            catch (Exception exc)
            {
                tcs.SetException(exc);
            }

            return tcs.Task;
        }

        public static Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request)
        {
            var tcs = new TaskCompletionSource<HttpWebResponse>();

            try
            {
                request.BeginGetResponse(iar =>
                    {
                        try
                        {
                            var response = (HttpWebResponse)request.EndGetResponse(iar);
                            tcs.SetResult(response);
                        }
                        catch (Exception exc)
                        {
                            tcs.SetException(exc);
                        }
                    }, null);
            }
            catch (Exception exc)
            {
                tcs.SetException(exc);
            }

            return tcs.Task;
        }
        

    }
}
