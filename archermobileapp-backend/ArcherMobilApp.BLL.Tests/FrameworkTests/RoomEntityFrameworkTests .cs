using System.Linq;
using System.Threading.Tasks;

using AutoFixture;
using Xunit;

using Archer.AMA.DAL.Contract;
using Archer.AMA.Entity;
using ArcherMobilApp.BLL.Tests.Helpers;
using ArcherMobilApp.DAL.MsSql;
using ArcherMobilApp.DAL.MsSql.Models;

namespace ArcherMobilApp.BLL.Tests.FrameworkTests
{
    public class RoomEntityFrameworkTests
    {
        [Fact(DisplayName = "Check add room to Entity Framework")]
        public async Task AddRoom_ToDb_ReturnRoomAsync()
        {
            //Arrange
            IRoomRepository rep = new TestRepositoryFactory<RoomRepository>().Create();
            var countRooms = (await rep.AllAsync(null, null)).Count();
            RoomEntity d = new Fixture()
                .Build<RoomEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Name, "Title test")
                .With(s => s.LinkToGoogleCalendar, @"https://googleCalendar.com")
                .With(s => s.Description, @"Description")
                .With(s => s.PhysicalAddress, @"PhysicalAddress")
                .Create();

            //Act
            var d1 = await rep.SaveAsync(d, null);

            //Assert
            Assert.Equal(0, countRooms);
            var countRoomsActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countRooms + 1, countRoomsActual);

            Assert.Single(rep.AllAsync(null, null).Result);
            Assert.Equal("Title test", d1.Name);
        }

        [Fact(DisplayName = "Check update room to Entity Framework")]
        public async Task UpdateRoom_InDb_ReturnRoomAsync()
        {
            //Arrange
            IRoomRepository rep = new TestRepositoryFactory<RoomRepository>().Create();
            var countRooms = (await rep.AllAsync(null, null)).Count();
            RoomEntity newRoom = new Fixture()
                .Build<RoomEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Name, "Title test")
                .With(s => s.LinkToGoogleCalendar, @"https://googleCalendar.com")
                .With(s => s.Description, @"Description")
                .With(s => s.PhysicalAddress, @"PhysicalAddress")
                .Create();

            RoomEntity updRoom = new Fixture()
                .Build<RoomEntity>()
                .With(s => s.Id, 1)
                .With(s => s.Name, "Title test updated")
                .With(s => s.LinkToGoogleCalendar, @"https://googleCalendar1.com")
                .With(s => s.Description, @"Description 1")
                .With(s => s.PhysicalAddress, @"PhysicalAddress 1")
                .Create();

            //Act
            var addedNewRoom = await rep.SaveAsync(newRoom, null);

            var upd = await rep.SaveAsync(updRoom, null);

            //Assert
            Assert.Equal(0, countRooms);
            var countRoomsActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countRooms + 1, countRoomsActual);

            Assert.Equal("Title test updated", upd.Name);
            Assert.Equal("Description 1", upd.Description);
            Assert.Equal("PhysicalAddress 1", upd.PhysicalAddress);
            Assert.Equal(@"https://googleCalendar1.com", upd.LinkToGoogleCalendar);
        }

        [Fact(DisplayName = "Check remove room to Entity Framework")]
        public async Task RemoveRoom_FromDb_ReturnBoolAsync()
        {
            //Arrange
            IRoomRepository rep = new TestRepositoryFactory<RoomRepository>().Create();
            var countRooms = (await rep.AllAsync(null, null)).Count();
            RoomEntity newRoom = new Fixture()
                .Build<RoomEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Name, "Title test")
                .With(s => s.LinkToGoogleCalendar, @"https://googleCalendar.com")
                .With(s => s.Description, @"Description")
                .With(s => s.PhysicalAddress, @"PhysicalAddress")
                .Create();

            var d1 = await rep.SaveAsync(newRoom, null);

            //Act
            var result = await rep.DeleteAsync(d1.Id, null);

            //Assert
            Assert.Equal(0, countRooms);
            var countRoomsActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countRooms, countRoomsActual);

            //Assert Repo
            Assert.True(result);
        }

        [Fact(DisplayName = "Check add the same room to Entity Framework")]
        public async Task AddThesameRoom_ToDb_ReturnRoomAsync()
        {
            //Arrange
            IRoomRepository rep = new TestRepositoryFactory<RoomRepository>().Create();
            var countRooms = (await rep.AllAsync(null, null)).Count();
            RoomEntity newRoom = new Fixture()
                .Build<RoomEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Name, "Title test")
                .With(s => s.LinkToGoogleCalendar, @"https://googleCalendar.com")
                .With(s => s.Description, @"Description")
                .With(s => s.PhysicalAddress, @"PhysicalAddress")
                .Create();
            var d1 = await rep.SaveAsync(newRoom, null);

            //Act
            var d2 = await rep.SaveAsync(d1, null);

            //Assert
            Assert.Equal(0, countRooms);
            var countRoomsActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countRooms + 1, countRoomsActual);
        }
    }
}
