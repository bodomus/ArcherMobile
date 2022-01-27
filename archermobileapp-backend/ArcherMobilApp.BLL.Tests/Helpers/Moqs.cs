using System.Collections.Generic;
using System.Linq;
using Archer.AMA.DAL.Contract;
using Archer.AMA.Entity;
using AutoFixture;
using Moq;

using ArcherMobilApp.BLL.Contracts;
using ArcherMobilApp.BLL.Models;
using ArcherMobilApp.DAL.MsSql.Models;
using IUserRepository = ArcherMobilApp.DAL.MsSql.Contract.IUserRepository;

namespace ArcherMobilApp.BLL.Tests.Helpers
{
    public static class Moqs
    {
        public static Mock<IUsersService> UsersServiceMoq()
        {
            var fixture = new Fixture();

            var moq = new Mock<IUsersService>(MockBehavior.Strict);
            moq.Setup(s => s.CreateUserAsync(It.IsAny<string>(), It.IsAny<User>()))
              .ReturnsAsync(fixture.Build<bool>().Create());
            moq.Setup(s => s.GetUserAsync(It.IsAny<string>()))
             .ReturnsAsync(fixture.Build<User>().Create());
            moq.Setup(s => s.UpdateUserAsync(It.IsAny<User>()))
              .ReturnsAsync(true);
            moq.Setup(s => s.DeleteUserAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
            moq.Setup(s => s.GetUsersListAsync(It.IsAny<int>(), It.IsAny<int>()))
             .ReturnsAsync(fixture.Build<User>().CreateMany(20));

            return moq;
        }

        public static Mock<IUserRepository> UsersReposirotyMoq(ComplexUserEntity userEntity)
        {
            var fixture = new Fixture();

            var moq = new Mock<IUserRepository>(MockBehavior.Strict);
            moq.Setup(s => s.CreateUserAsync(It.IsAny<string>(), It.IsAny<UserEntity>()))
              .ReturnsAsync(true);
            moq.Setup(s => s.GetUserAsync(It.IsAny<string>()))
             .ReturnsAsync(userEntity);
            moq.Setup(s => s.UpdateUserAsync(It.IsAny<UserEntity>()))
              .ReturnsAsync(true);
            moq.Setup(s => s.DeleteUserAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
            //moq.Setup(s => s.GetUsersAsync(It.IsAny<int>(), It.IsAny<int>()))
            // .ReturnsAsync(fixture.Build<ComplexUserEntity>().CreateMany(20));

            return moq;
        }

        
        public static Mock<IDocumentRepository> DocumentRepositoryMoq(DocumentEntity documentEntity, string user)
        {
            var fixture = new Fixture();
            var docs = fixture.Build<IEnumerable<DocumentEntity>>().CreateMany<DocumentEntity>(20);
            var documents = new List<DocumentEntity>
            {
                new DocumentEntity { Id=1, Description = "", Title = ""},
                new DocumentEntity { Id=2, Description = "", Title = ""}
            };
            var t1 = new Fixture().Build<DocumentEntity>().With(s => s.Title, @"asdasd").CreateMany(20);

            var t = fixture.Build<DocumentEntity>().CreateMany<DocumentEntity>().ToList();
            var moq = new Mock<IDocumentRepository>(MockBehavior.Default);
            moq.Setup(s => s.SaveAsync(It.IsAny<DocumentEntity>(), It.IsAny<string>())).ReturnsAsync(documentEntity);
            moq.Setup(s => s.DeleteAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            moq.Setup(s => s.GetByIdAsync(It.IsAny<int>(), It.IsAny<string>(), null)).ReturnsAsync(fixture.Build<DocumentEntity>().Create());
            moq.Setup(s => s.AllAsync(null, null))
                .ReturnsAsync(fixture.Build<DocumentEntity>().CreateMany(2));
            return moq;
        }

        public static Mock<IRoomRepository> RoomRepositoryMoq(RoomEntity roomEntity, string user)
        {
            var fixture = new Fixture();
            var rooms = fixture.Build<IEnumerable<RoomEntity>>().CreateMany<RoomEntity>(20);
            var documents = new List<RoomEntity>
            {
                new RoomEntity { Id=1, Description = "Description 1", Name = "Name 1"},
                new RoomEntity { Id=2, Description = "Description 2", Name = "Name 2"}
            };
            var t1 = new Fixture().Build<RoomEntity>().With(s => s.Name, @"asdasd").CreateMany(20);

            var t = fixture.Build<RoomEntity>().CreateMany<RoomEntity>().ToList();
            var moq = new Mock<IRoomRepository>(MockBehavior.Default);
            moq.Setup(s => s.SaveAsync(It.IsAny<RoomEntity>(), It.IsAny<string>())).ReturnsAsync(roomEntity);
            moq.Setup(s => s.DeleteAsync(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);
            moq.Setup(s => s.GetByIdAsync(It.IsAny<int>(), It.IsAny<string>(), null)).ReturnsAsync(fixture.Build<RoomEntity>().Create());
            moq.Setup(s => s.AllAsync(null, null))
                .ReturnsAsync(fixture.Build<RoomEntity>().CreateMany(2));
            return moq;
        }
    }
}
