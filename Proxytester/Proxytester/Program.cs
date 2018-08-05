using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Net;
using System.Diagnostics;

namespace Proxytester
{
    class Program
    {
        static void Main(string[] args)
        {
            string proxy = ""; //Change this your desired proxy.
            string testUrl = ""; //Change this to your desired website. 
            TestProxy(testUrl,proxy);
            Console.ReadLine();
        }


        async static void TestProxy(string url, string prox)
        {
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            if (prox == "NONE")
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36");
                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                }                    
            }
            else
            {

                string[] p = Regex.Split(prox, ":");
                string host = p[0];
                string port = p[1];
                string user = "";
                string password = "";
                Boolean credentials = false;
                try
                {
                    user = p[2];
                    password = p[3];
                }
                catch (IndexOutOfRangeException)
                {
                    user = "";
                    password = "";
                }

                if (user != "")
                {
                    credentials = true;
                }


                var proxy = new WebProxy()
                {
                    Address = new Uri("http://" + host + ":" + port),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = credentials,
                    Credentials = new NetworkCredential(userName: user, password: password)
                };
                var httpClientHandler = new HttpClientHandler()
                {
                    Proxy = proxy,
                };

                using (var client = new HttpClient(handler: httpClientHandler, disposeHandler: true))
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.99 Safari/537.36");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    sw.Start();
                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        sw.Stop();
                        Console.WriteLine(response.StatusCode);
                        Console.WriteLine(sw.ElapsedMilliseconds);
                        Console.WriteLine(response.RequestMessage);

                        //Code Below is used to Test if connection is connected to Proxy. 
                        //using (HttpContent content = response.Content)
                        //{
                        //     string responseBody = await response.Content.ReadAsStringAsync();
                        //     Console.WriteLine(responseBody);
                        //}
                    }
                }
            }


        }  
    }
}
