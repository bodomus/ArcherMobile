using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace ArcherMobilApp.BLL.Tests.Tools
{
    public static class ApiTestServer
    {
        public const string ApiIndetifier = "abffd6fa448d4a04b5e6c9b4c4f04870";
        public const string BaseAddress = "http://localhost:64009/v1/";

        public static HttpClient CreateClient(string fakeIp = null)
        {
            var httpClient = TestServer.CreateClient();
            if (!string.IsNullOrEmpty(BaseAddress))
                httpClient.BaseAddress = new Uri(BaseAddress);

            if (!string.IsNullOrEmpty(fakeIp))
                httpClient.DefaultRequestHeaders.Add("test-fake-ip", fakeIp);

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");

            return httpClient;
        }

        private static readonly object obj = new object();
        private static TestServer _testServer;
        public static TestServer TestServer
        {
            get
            {
                if (_testServer == null)
                {
                    lock (obj)
                    {
                        if (_testServer == null)
                        {
                            var builder = new WebHostBuilder()
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseStartup(typeof(ApiTestStartup));

                            _testServer = new TestServer(builder);
                        }
                    }
                }

                return _testServer;
            }
        }
    }
}