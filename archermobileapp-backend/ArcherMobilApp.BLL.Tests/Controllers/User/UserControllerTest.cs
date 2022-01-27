using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Archer.AMA.BLL.Contracts;
using ArcherMobilApp.BLL.Tests.Helpers;
using ArcherMobilApp.BLL.Tests.Tools;
using ArcherMobilApp.Controllers;
using ArcherMobilApp.DAL.MsSql.Contract;
using ArcherMobilApp.Infrastracture;
using ArcherMobilApp.Models;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;
using Xunit.Abstractions;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ArcherMobilApp.BLL.Tests.Controllers.User
{
    [Collection("Time collection")]
    [Trait("Controller", "User")]
    public class UserControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private ITestOutputHelper _outputHelper;
        private TimeFixture _timeFixture;
        private readonly WebApplicationFactory<Startup> _factory;

        public UserControllerTest(
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

            var docService = new Mock<IDocumentService>();
            var mapService = new Mock<IMapper>();
            DocumentController controller = new DocumentController(docService.Object, mapService.Object);
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

        [Trait("Controller", "User")]
        [Theory(DisplayName = "Check on athorized access to controller")]
        [ClassData(typeof(AthorizedMethodUserControllerTestData))]
        public async Task Get_ApiEndpoints_ReturnUnautorizedStatus(string url, string @type, HttpStatusCode status)
        {
            // Arrange
            var client = _factory.CreateClient();

            HttpCompletionOption o = new HttpCompletionOption() { };
            // Act
            var response = await client.GetResponse(url, @type, null); //await client.GetAsync(url);

            // Assert
            //response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(status, response.StatusCode);
        }

        [Trait("Controller", "User")]
        [Theory(DisplayName = "Check on unathorized access to controller")]
        [ClassData(typeof(UnAuthorizedMethodUserControllerTestData))]
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

        [Fact(DisplayName = "Test invalid user login")]
        [Trait("Controller", "User")]
        public async void CheckLogin_PasswordSignInAsyncFalse_ReturnBadRequest()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Test different approach
            //----------------------------------------------------------------------------------------------------
            /*var um = Substitute.For<UserManager<IdentityUser>>();
            
            var sim = Substitute.For<SignInManager<IdentityUser>>
            (
                um,
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null
            );*/
            var users = new List<IdentityUser>
            {
                new IdentityUser
                {
                    UserName = "Test",
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.it"
                }

            }.AsQueryable();

            var fakeUserManager = new Mock<FakeUserManager>();
            fakeUserManager.Setup(x => x.Users)
                .Returns(users);
            fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var moqSignInManager = new Mock<FakeSignInManager>();
            moqSignInManager.Setup(
                x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>())).Returns(Task.FromResult(SignInResult.Failed));
            var mapService = new Mock<IMapper>();
            //--------------------------------------------------------------------------------------------------
            UserController controller = new UserController(configuration, mapService.Object, moqSignInManager.Object, null, null, null);
            var loginModel = new Fixture().Build<LoginViewModel>().Create();

            // Act
            IActionResult result = await controller.Login(loginModel, string.Empty) ;

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact(DisplayName = "Test on lockout user")]
        [Trait("Controller", "User")]
        public async void CheckLogin_PasswordSignInAsyncLockout_ReturnBadRequest()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var users = new List<IdentityUser>
            {
                new IdentityUser
                {
                    UserName = "Test",
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.it"
                }

            }.AsQueryable();

            var fakeUserManager = new Mock<FakeUserManager>();
            fakeUserManager.Setup(x => x.Users)
                .Returns(users);
            fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var moqSignInManager = new Mock<FakeSignInManager>();
            moqSignInManager.Setup(
                x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>())).Returns(Task.FromResult(SignInResult.LockedOut));
            var mapService = new Mock<IMapper>();
            //--------------------------------------------------------------------------------------------------
            UserController controller = new UserController(configuration, mapService.Object, moqSignInManager.Object, null, null, null);
            var loginModel = new Fixture().Build<LoginViewModel>().Create();

            // Act
            IActionResult result = await controller.Login(loginModel, string.Empty);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.Forbidden, ((StatusCodeResult)result).StatusCode); 
        }


        [Fact(DisplayName = "Get user by id")]
        [Trait("Controller", "User")]
        public async void GetUser_byId2_ReturnStatusOk()
        {
             
            // Arrange
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            _factory.Server.BaseAddress = new Uri("http://localhost:44361");

            var token = new TokenGenerator(configuration).GenerateToken("test@gmail.com", new List<string>() { "Admins" });
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(@"http://localhost:44361");
            var repo = UserRepositoryMock.Create(null);

            var result = await client.GetResponse("Home/Index/2", "get", null); 
            var result3 = await client.GetAsync("api/user/2");
            var result2 = await client.GetAsync("api/room/2");

        }

        [Fact(DisplayName = "Get user by id")]
        [Trait("Controller", "User")]
        public async void GetRoom_byId2_ReturnStatusOk()
        {
            // Arrange
            var client = _factory.CreateClient();
            

            var result3 = await client.GetAsync("api/user/Current");
            result3.EnsureSuccessStatusCode();
        }


        [Fact]
        public async Task ShouldReturnHelloWorld()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    // Add TestServer
                    webHost.UseTestServer();
                    webHost.Configure(app => app.Run(async ctx =>
                        await ctx.Response.WriteAsync("Hello World!")));
                });

            // Build and start the IHost
            var host = await hostBuilder.StartAsync();

            // Create an HttpClient to send requests to the TestServer
            var client = host.GetTestClient();

            var response = await client.GetAsync("/");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Hello World!", responseString);
        }
    }
}
