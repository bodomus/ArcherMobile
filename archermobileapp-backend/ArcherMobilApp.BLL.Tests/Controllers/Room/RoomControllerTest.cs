using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

using Moq;

using Archer.AMA.BLL.Contracts;
using ArcherMobilApp.BLL.Tests.Controllers.Document;
using ArcherMobilApp.BLL.Tests.Helpers;
using ArcherMobilApp.BLL.Tests.Tools;
using ArcherMobilApp.Controllers;
using ArcherMobilApp.DAL.MsSql.Contract;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Xunit;
using Xunit.Abstractions;

namespace ArcherMobilApp.BLL.Tests.Controllers.Room
{
    [Collection("Time collection")]
    [Trait("Controller", "Room")]
    public class RoomControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ITestOutputHelper _outputHelper;
        private readonly TimeFixture _timeFixture;
        private readonly WebApplicationFactory<Startup> _factory;

        public RoomControllerTest(
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
            var docService = new Mock<IRoomService>();
            RoomController controller = new RoomController(docService.Object);
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
        [Trait("Controller", "Room")]
        [Theory(DisplayName = "Check on unauthorized access to controller")]
        [ClassData(typeof(AuthorizedMethodRoomControllerTestData))]
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

        [Trait("Controller", "Document")]
        [Theory(DisplayName = "Check on unauthorized access to document controller")]
        [ClassData(typeof(UnAuthorizedMethodDocumentControllerTestData))]
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

        [Fact(DisplayName = "Get document by id")]
        [Trait("Controller", "Document")]
        public async void GetDocument_byId2_ReturnStatusOk()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var mapService = new Mock<IMapper>();
            DocumentController controller = new DocumentController(DocumentMocks.DocumentServiceMoq().Object, mapService.Object);

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            
            // Act
            var result = controller.Get();
            //var result = await client.GetResponse("api/user/", "get", null); //await client.GetAsync(url);
            // Assert
            Assert.IsType<StatusCodeResult>(result);
        }
    }
}
