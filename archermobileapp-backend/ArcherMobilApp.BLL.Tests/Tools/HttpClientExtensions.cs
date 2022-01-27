using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ArcherMobilApp.BLL.Tests.Tools
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> GetResponse(this HttpClient client,string url, string typeRequest, HttpContent? content)
        {
            switch (typeRequest.ToLower())
            {
                case "get":
                {
                    return await client.GetAsync(url);
                }
                case "put":
                {
                    return await client.PutAsync(url, content);
                }
                case "post":
                {
                    return await client.PostAsync(url, content);
                }
                case "delete":
                {
                    return await client.DeleteAsync(url);
                }
                default:
                    throw new ArgumentException("Mismatch parameters.");
            }
        }
    }
}
