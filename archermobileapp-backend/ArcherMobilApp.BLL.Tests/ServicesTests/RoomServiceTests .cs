//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Archer.AMA.DAL.Contract;
//using Archer.AMA.DTO;
//using Archer.AMA.Entity;
//using ArcherMobilApp.BLL.Tests.Helpers;

//using ArcherMobilApp.DAL.MsSql;
//using ArcherMobilApp.DAL.MsSql.Models;
//using AutoFixture;
//using Moq;
//using Newtonsoft.Json;
//using Xunit;

//namespace ArcherMobilApp.BLL.Tests.ServicesTests
//{
//    public class RoomServiceTests
//    {
//        [Theory(DisplayName = "Check create room")]
//        [InlineData("New room 1")]
//        public void CreateRoom_ReturnRoom(string roomName)
//        {
//            //Arrange
//            var mapper = Mapper.GetAutoMapper();
//            var room = Fixtures.RoomDTOFixture(roomName);
//            var userId = new Fixture().Create<Guid>().ToString();
//            var roomRepoMoq = ArcherMobilApp.BLL.Tests.Helpers.Moqs.RoomRepositoryMoq(mapper.Map<RoomEntity>(room), userId);
//            var roomSvc = new RoomService(roomRepoMoq.Object, mapper);

//            //Act
//            var newRoom = roomSvc.SaveAsync(room, userId).Result;

//            ////Assert
//            var actual = JsonConvert.SerializeObject(room);
//            var expected = JsonConvert.SerializeObject(newRoom);
//            roomRepoMoq.Verify();
//            Assert.Equal(expected.Trim(), actual.Trim());
//        }

//        [Theory(DisplayName = "Check delete room")]
//        [InlineData("Room", 2)]
//        public void DeleteRoom_id2_TrueReturned(string roomName, int id)
//        {
//            // "123D9C71-25CF-4CFB-ABE8-CCE7FE12969D"
//            //Arrange
//            var mapper = Mapper.GetAutoMapper();
//            var room = Fixtures.RoomDTOFixture(roomName);
//            var user = new Fixture().Create<Guid>().ToString();

//            var roomRepoMoq = ArcherMobilApp.BLL.Tests.Helpers.Moqs.RoomRepositoryMoq(mapper.Map<RoomEntity>(room), user);
//            var roomSvc = new RoomService(roomRepoMoq.Object, mapper);
//            //Act
//            var newRoom = roomSvc.DeleteAsync(id, user).Result;

//            ////Assert
//            Assert.True(newRoom);
//            Assert.IsAssignableFrom<RoomDTO>(newRoom);
//            roomRepoMoq.Verify();
//        }

//        [Theory(DisplayName = "Check update room")]
//        [InlineData("New room update 1", 1)]
//        public void UpdateRoom_id1_ReturnRoom(string roomName, int? id)
//        {
//            //Arrange
//            var mapper = Mapper.GetAutoMapper();

//            var room = Fixtures.RoomDTOFixture(roomName, id);

//            var user = new Fixture().Create<Guid>().ToString();

//            var roomRepoMoq = Moqs.RoomRepositoryMoq(mapper.Map<RoomEntity>(room), user);
//            var roomSvc = new RoomService(roomRepoMoq.Object, mapper);

//            //Act
//            var newRoom = roomSvc.SaveAsync(room, user).Result;

//            ////Assert
//            var actual = JsonConvert.SerializeObject(room);
//            var expected = JsonConvert.SerializeObject(newRoom);
//            Assert.Equal(expected.Trim(), actual.Trim());
//            Assert.IsAssignableFrom<RoomDTO>(newRoom);
//            roomRepoMoq.Verify();
//        }

//        [Fact(DisplayName = "Check get 20 rooms", Skip = "Need check Mock")]
//        public void GetRooms_Return20Rooms()
//        {
//            //Arrange
//            var mapper = Mapper.GetAutoMapper();

//            var room = Fixtures.RoomDTOFixture(string.Empty, null);

//            var user = new Fixture().Create<Guid>().ToString();

//            var roomRepoMoq = Moqs.RoomRepositoryMoq(mapper.Map<RoomEntity>(room), user);
//            var roomSvc = new RoomService(roomRepoMoq.Object, mapper);

//            //Act
//            var rooms = roomSvc.AllAsync(user).Result;

//            ////Assert

//            roomRepoMoq.Verify(x => x.AllAsync("", null), Times.Exactly(20));

//            Assert.Equal(20, rooms.Count());
//            Assert.IsAssignableFrom<IEnumerable<RoomDTO>>(rooms);
//        }

//        [Theory(DisplayName = "Check get room")]
//        [InlineData(2)]
//        public void GetRoom_ById2_ReturnRoomAsync(int id)
//        { 
//            //Arrange
//            var fixture = new Fixture();
//            var model = fixture.Create<RoomEntity>();
//            var fixture1 = new Fixture();
//            var model1 =
//                fixture1
//                    .Build<RoomEntity>()
//                    .Without(x => x.Id)
//                    .Create();

//            var mapper = Mapper.GetAutoMapper();

//            var room = Fixtures.RoomDTOFixture(string.Empty, null);

//            var user = new Fixture().Create<Guid>().ToString();

//            var roomRepoMoq = Moqs.RoomRepositoryMoq(mapper.Map<RoomEntity>(room), user);
//            var roomSvc = new RoomService(roomRepoMoq.Object, mapper);

//            //Act
//            var roomDto = roomSvc.GetByIdAsync(id, user, null).Result;

//            ////Assert
//            Assert.IsType<RoomDTO>(roomDto);
//            roomRepoMoq.Verify();
//        }
//    }
//}
