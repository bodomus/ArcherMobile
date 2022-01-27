using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

using Xunit;
using Xunit.Abstractions;
using AutoFixture;
using Newtonsoft.Json;

using ArcherMobilApp.BLL.Tests.Tools;
using Archer.AMA.DTO;

namespace ArcherMobilApp.BLL.Tests.IntegrationTests
{
    [Collection("JwtToken collection")]
    [TestCaseOrderer("ArcherMobilApp.BLL.Tests.IntegrationTests.PriorityOrderer", "ArcherMobilApp.BLL.Tests")]
    public class RoomIntegrationTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private ITestOutputHelper _outputHelper;
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly JwtTokenFixture _jwtTokenFixture;

        public RoomIntegrationTest(
            WebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper,
            JwtTokenFixture jwtTokenFixture)
        {
            _outputHelper = outputHelper;
            _factory = factory;
            _jwtTokenFixture = jwtTokenFixture;
        }

        [Fact(DisplayName = "Get room by id"), TestPriority(2)]
        [Trait("Integration", "Room")]
        public async void GetRoom_byId2_ReturnStatusOk()
        {
            // Arrange
            _factory.Server.BaseAddress = new Uri("http://localhost:44361");
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(@"http://localhost:44361");
            var rooms = await client.GetResponse("api/room/", "get", null);
            Assert.NotNull(rooms);
        }

        [Fact(DisplayName = "Get all rooms"), TestPriority(1)]
        [Trait("Integration", "Room")]
        public async void GetAllRooms_ReturnStatusOk()
        {
            // Arrange
            var token = _jwtTokenFixture.Token;
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

            //Action
            HttpResponseMessage rooms = await client.GetAsync("api/room/");

            //Assert
            Assert.NotNull(rooms);
            Assert.Equal(HttpStatusCode.OK, rooms.StatusCode);
        }

        [Fact(DisplayName = "Create room"), TestPriority(1)]
        [Trait("Integration", "Room")]
        public async void CreateRoom_ReturnStatusCreate()
        {
            // Arrange
            var token = _jwtTokenFixture.Token;
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

            //Action
            //Add new record
            var newRoom = new Fixture()
                .Build<RoomDTO>().With(a => a.Id, (int?)null)
                .Create();
            var resultNew = await client.GetResponse("api/room", "post", ContentHelper.GetStringContent(newRoom));
            var value = await resultNew.Content.ReadAsStringAsync();
            var room = JsonConvert.DeserializeObject<RoomDTO>(value);
            //Assert
            Assert.Equal(HttpStatusCode.Created, resultNew.StatusCode);
            Assert.NotNull(room);
            Assert.IsAssignableFrom<RoomDTO>(room);
            Assert.NotNull(room.Id);
        }

        [Fact(DisplayName = "Get room by not exist id"), TestPriority(1)]
        [Trait("Integration", "Room")]
        public async void GetRoom_byNotExistId_ReturnStatusOk()
        {
            // Arrange
            var token = _jwtTokenFixture.Token;
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

            //Action
            HttpResponseMessage rooms = await client.GetAsync("api/room/-1");

            //Assert
            Assert.NotNull(rooms);
            Assert.Equal(HttpStatusCode.NotFound, rooms.StatusCode);
        }

        [Fact(DisplayName = "Delete room by exist id"), TestPriority(1)]
        [Trait("Integration", "Room")]
        public async void DeleteRoom_byExistId_ReturnStatusOk()
        {
            // Arrange
            int? roomId;

            var token = _jwtTokenFixture.Token;
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

            HttpResponseMessage rooms = await client.GetAsync("api/room/");
            var value = await rooms.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<IEnumerable<RoomDTO>>(value)?.FirstOrDefault();

            //Action
            if (obj == null)
            {
                //Add new record
                var newRoom = new Fixture()
                    .Build<RoomDTO>().With(a => a.Id, (int?)null)
                    .Create();
                var resultNew = await client.GetResponse("api/room", "post", ContentHelper.GetStringContent(newRoom));
                Assert.Equal(HttpStatusCode.OK, resultNew.StatusCode);
                var r = await client.GetAsync("api/room/");
                roomId = JsonConvert.DeserializeObject<IEnumerable<RoomDTO>>(value)?.FirstOrDefault()?.Id;
            }
            else
            {
                roomId = obj.Id;
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);
            HttpResponseMessage response = await client.GetResponse($"api/room/{roomId}", "delete", null);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "Delete room by not exist id"), TestPriority(1)]
        [Trait("Integration", "Room")]
        public async void DeleteRoom_byNitExistId_ReturnStatusNotFound()
        {
            // Arrange
            int? roomId;

            var token = _jwtTokenFixture.Token;
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

            //Action

            HttpResponseMessage response = await client.GetResponse($"api/room/-1", "delete", null);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact(DisplayName = "Update room if ID room exists"), TestPriority(1)]
        [Trait("Integration", "Room")]
        public async void UpdateRoom_IdExist_ReturnStatusOk()
        {
            // Arrange
            var token = _jwtTokenFixture.Token;
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);

            //Action
            //Add new record
            var oldRoom = new Fixture()
                .Build<RoomDTO>()
                .With(a => a.Id, (int?)null)
                .With(a => a.Name, "Old room")
                .Create();
            var resultOld = await client.GetResponse("api/room", "post", ContentHelper.GetStringContent(oldRoom));
            var valueOld = await resultOld.Content.ReadAsStringAsync();
            var roomOld = JsonConvert.DeserializeObject<RoomDTO>(valueOld);
            var roomId = roomOld.Id ?? 0;

            //Create new room
            var newRoom = new Fixture()
                .Build<RoomDTO>()
                .With(a => a.Id, roomId)
                .With(a => a.Name, "New room")
                .Create();

            var resultNew = await client.GetResponse("api/room", "put", ContentHelper.GetStringContent(newRoom));
            var valueNew = await resultNew.Content.ReadAsStringAsync();
            var roomNew = JsonConvert.DeserializeObject<RoomDTO>(valueNew);

            //Assert
            Assert.Equal(HttpStatusCode.OK, resultNew.StatusCode);
            Assert.NotNull(roomNew);
            Assert.IsAssignableFrom<RoomDTO>(roomNew);
            Assert.Equal("New room", roomNew.Name);
        }

        [Fact(DisplayName = "Update room if ID room not exists"), TestPriority(1)]
        [Trait("Integration", "Room")]
        public async void UpdateRoom_idNotExist_ReturnStatusNotFound()
        {
            // Arrange
            var token = _jwtTokenFixture.Token;
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Value);
            var newRoom = new Fixture()
                .Build<RoomDTO>()
                .With(a => a.Id, -1)
                .With(a => a.Name, "New room")
                .Create();
            //Action
            var resultNew = await client.GetResponse("api/room", "put", ContentHelper.GetStringContent(newRoom));

            //Assert
            Assert.NotNull(resultNew);
            Assert.Equal(HttpStatusCode.NotFound, resultNew.StatusCode);
        }
    }
}
