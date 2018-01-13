using Fiddler;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ProxyTetriSwitch
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
            FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
            CONFIG.IgnoreServerCertErrors = true;

            FiddlerApplication.Startup(8008, false, false, true);

            client.DefaultRequestHeaders.TryAddWithoutValidation("password", "302569db917c44a8bfb3b00267cb927a");

            Console.ReadLine();
        }

        private static void FiddlerApplication_BeforeResponse(Session oSession)
        {
            if (oSession.fullUrl == "http://sega.jp/")
            {
                oSession.ResponseHeaders.SetStatus(200, "OK");
                oSession.ResponseHeaders.Remove("Location");
                
                var bytes = client.GetByteArrayAsync("http://video.example.com/").Result;
                oSession.ResponseHeaders["Content-Length"] = bytes.Length.ToString();
                oSession.responseBodyBytes = bytes;
            }
        }

        private static void FiddlerApplication_BeforeRequest(Session oSession)
        {
            if (oSession.fullUrl.Contains("http://sega.jp/"))
            {
                oSession.bBufferResponse = true;
            }
        }
    }
}
