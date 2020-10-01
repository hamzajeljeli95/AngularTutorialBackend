using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace CommonLibrary
{
    public class WebCommunicator
    {
        public static string ReadFromURI(string url, HttpMethod method, String Body, out HttpStatusCode status,String Token)
        {
           
            String resp = String.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method.Method;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            if(!Token.Length.Equals(0))
            {
                request.Headers.Add(HttpRequestHeader.Authorization,
                "Credentials " + Token);
            }
            if (Body.Equals(String.Empty))
            {
                request.ContentLength = 0;
            }
            else
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Body);
                }
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    status = response.StatusCode;
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            resp = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse.StatusCode.Equals(null))
                    {
                        throw new Exception("Could not read HTTP Status code, Endpoint is unreachable.");
                    }
                    else
                    {
                        status = httpResponse.StatusCode;
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            resp = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                status = HttpStatusCode.BadGateway;
                resp = String.Empty;
            }

            return resp;
        }
        public static string ReadFromURI(string url, HttpMethod method, String Body, out HttpStatusCode status)
        {
            return ReadFromURI(url, method, Body, out status,String.Empty);
        }
        public static string ReadFromURI(string url, HttpMethod method, String Body)
        {
            return ReadFromURI(url, method, Body, out HttpStatusCode code);
        }
        public static string ReadFromURI(string url, HttpMethod method)
        {
            return ReadFromURI(url, method, String.Empty);
        }
        public static string ReadFromURI(string url)
        {
            return ReadFromURI(url,HttpMethod.Get);
        }
    }
}
