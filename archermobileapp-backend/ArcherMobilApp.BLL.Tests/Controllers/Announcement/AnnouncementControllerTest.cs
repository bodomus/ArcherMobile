using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Archer.AMA.BLL.Contracts;
using ArcherMobilApp.BLL.Tests.Tools;
using ArcherMobilApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using Xunit.Abstractions;

//ExampleAppTestFixture

namespace ArcherMobilApp.BLL.Tests.Controllers.Announcement
{
    [Collection("Time collection")]
    [Trait("Controller", "Announcement")]
    public class AnnouncementControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly TimeFixture _timeFixture;
        private readonly WebApplicationFactory<Startup> _factory;

        public AnnouncementControllerTest(
            WebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper, 
            TimeFixture timeFixture)
        {
            _outputHelper = outputHelper;
            _timeFixture = timeFixture;
            _factory = factory;
        }

        [Fact(DisplayName = "Check authorized attribute on all method")]
        public async Task UnautorizedGetAttribute()
        {
            // Arrange
            int actualAutAttr = 0;
            var docService = new Mock<IAnnouncementService>();
            AnnouncementController controller = new AnnouncementController(docService.Object);
            var methods = controller.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            int expect = methods.Length;
            _outputHelper.WriteLine("Get total count method with authorized attribute ");
            foreach (var m in methods)
            {
                var a1 = m.GetCustomAttribute(typeof(AuthorizeAttribute));
                var aA = m.GetCustomAttributes(typeof(AuthorizeAttribute), true);
                actualAutAttr = actualAutAttr + (aA != null ? 1 : 0);
            }
            // Act
            var dt = _timeFixture.DateTime;

            // Assert
            Assert.Equal(expect, actualAutAttr);
        }

        /// <summary>
        /// Check returning status code from controller method
        /// if using unauthorized request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Trait("Controller", "Announcement")]
        [Theory(DisplayName = "Check on unauthorized access to controller")]
        [ClassData(typeof(AuthorizedMethodAnnouncementControllerTestData))]
        public async Task Get_ApiEndpoints_ReturnUnauthorizedStatus(string url, string @type, HttpStatusCode status)
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetResponse(url, @type, null); //await client.GetAsync(url);
            // Assert
            //response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(status, response.StatusCode);
        }

        [Trait("Controller", "Announcement")]
        [Theory(DisplayName = "Check on unauthorized access to announcement controller")]
        [ClassData(typeof(UnAuthorizedMethodAnnouncementControllerTestData))]
        public async Task Get_ApiEndpoints_ReturnStatus(string url, string @type, HttpStatusCode status)
        {
            // Arrange
            var client = _factory.CreateClient();

            HttpCompletionOption o = new HttpCompletionOption() { };
            // Act
            var response = await client.GetResponse(url, @type, null); //await client.GetAsync(url);

            // Assert
            Assert.NotEqual(status, response.StatusCode);
        }

        [Fact(DisplayName = "Get announcement by id")]
        [Trait("Controller", "Announcement")]
        public async void GetAnnouncement_byId2_ReturnStatusOk()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            AnnouncementController controller = new AnnouncementController(Helpers.AnnouncementMock.AnnouncementServiceMoq().Object);

            // Act
            
            var result = controller.Get();
            //var result = await client.GetResponse("api/user/", "get", null); //await client.GetAsync(url);
            // Assert
            Assert.IsType<StatusCodeResult>(result);
        }
    }
}
