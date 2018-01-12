using Fiddler;
using System;
using System.IO;
using System.Net.Http;

namespace ProxyTetriSwitch
{
    class Program
    {
        static void Main(string[] args)
        {
            FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
            FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
            CONFIG.IgnoreServerCertErrors = true;

            FiddlerApplication.Startup(8008, false, false, true);

            Console.ReadLine();
        }

        private static void FiddlerApplication_BeforeResponse(Session oSession)
        {
            if (oSession.fullUrl == "http://sega.jp/")
            {
                oSession.ResponseHeaders.SetStatus(200, "OK");
                oSession.ResponseHeaders.Remove("Location");

                //var bytes = new HttpClient().GetByteArrayAsync("http://example.com").Result;
                var bytes = File.ReadAllBytes("html/index.html");
                oSession.ResponseHeaders["Content-Length"] = bytes.Length.ToString();
                oSession.responseBodyBytes = bytes;
            }
        }

        private static void FiddlerApplication_BeforeRequest(Session oSession)
        {
            if (oSession.fullUrl == "http://sega.jp/")
            {
                oSession.bBufferResponse = true;
            }
        }
    }
}
