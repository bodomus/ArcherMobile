//using System.Net;
//using System.Reflection;
//using System.Threading.Tasks;
//using ArcherMobilApp.Controllers;

//using Archer.AMA.BLL.Contracts;
//using Archer.AMA.DTO;
//using ArcherMobilApp.BLL.Tests.Tools;
//using AutoFixture;
//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Xunit;
//using Xunit.Abstractions;

//namespace UnitTestApp.Tests
//{
//    [Collection("Time collection")]
//    [Trait("Controller", "Home")]
//    public class HomeControllerTests
//    {
//        private ITestOutputHelper _outputHelper;
//        private TimeFixture _timeFixture;

//        public HomeControllerTests(ITestOutputHelper outputHelper, TimeFixture timeFixture)
//        {
//            _outputHelper = outputHelper;
//            _timeFixture = timeFixture;
//        }

//        [Fact]
//        [Trait("Controller", "Home")]
//        public void IndexViewDataMessage()
//        {
//            ILogger<HomeController> logger = 

//            // Arrange
//            HomeController controller = new HomeController();

//            // Act
//            ViewResult result = controller.Index() as ViewResult;
            
//            // Assert
//            Assert.Equal("Hello world!", controller.ViewData["Message"]);
//        }

//        [Fact(DisplayName = "Check authorized attribute")]
//        public async Task UnautorizedGetData()
//        {
//            // Arrange
//            int actualAutAttr = 0;

//            var docService = new Mock<IDocumentService>();
//            var mapService = new Mock<IMapper>(); 
//            DocumentController controller = new DocumentController(docService.Object, mapService.Object);
//            var methods = controller.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
//            int expect = methods.Length;
//            _outputHelper.WriteLine("Get total count method with authorized attribute ");
//            foreach (var m in methods)
//            {
//                var a1 = m.GetCustomAttribute(typeof(AuthorizeAttribute));
//                var aA = m.GetCustomAttributes(typeof(AuthorizeAttribute), true);
//                actualAutAttr = actualAutAttr + (aA != null? 1: 0);
//            }


//            //var actualAttribute = controller.GetType().GetMethod(@"Get").GetCustomAttributes(typeof(AuthorizeAttribute), true);


//            DocumentDTO doc = new Fixture().Build<DocumentDTO>().With(s=> s.Id, (int?)1).Create();

//            // Act
//            var dt = _timeFixture.DateTime;
//            //var status = await controller.Delete((int)doc.Id);

//            // Assert
//            Assert.Equal(expect, actualAutAttr);
//            //Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType()); 
//            //Assert.Equal((int)HttpStatusCode.Unauthorized, 200);
//        }


//    }
//}
