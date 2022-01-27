using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ArcherMobilApp.BLL.Tests.Controllers.User
{
    public class HttpTests : IClassFixture<ExampleAppTestFixture>, IDisposable
    {
        readonly ExampleAppTestFixture _fixture;
        readonly HttpClient _client;

        public HttpTests(ExampleAppTestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            // fixture.Output = output;
            // fixture.Server.BaseAddress = new Uri(@"http://localhost:44361");
            _client = fixture.CreateClient();
            // _client.BaseAddress = new Uri(@"http://localhost:44361");

        }

        public void Dispose() => _fixture.Output = null;

        [Fact]
        public async Task CanCallApi()
        {
            var result = await _client.GetAsync("/Home/Index");

            result.EnsureSuccessStatusCode();

            var content = await result.Content.ReadAsStringAsync();
            Assert.Contains("Welcome", content);
        }
    }
}
