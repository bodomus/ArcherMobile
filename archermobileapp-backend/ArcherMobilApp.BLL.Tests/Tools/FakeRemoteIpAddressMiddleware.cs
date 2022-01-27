using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace ArcherMobilApp.BLL.Tests.Tools
{
    public class FakeRemoteIpAddressMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IPAddress fakeIpAddress = IPAddress.Parse("127.0.0.1");

        public FakeRemoteIpAddressMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Connection.RemoteIpAddress = httpContext.Request.Headers.TryGetValue("test-fake-ip", out var testIp) ? IPAddress.Parse(testIp) : fakeIpAddress;
            await next(httpContext);
        }
    }
}