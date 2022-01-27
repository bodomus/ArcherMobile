using Archer.AMA.DAL.Contract;
using ArcherMobilApp.DAL.MsSql.Models;
using AutoFixture;
using Moq;

namespace ArcherMobilApp.BLL.Tests.Helpers
{
    public static class UserRepositoryMock
    {
        public static Mock<IUserRepository> Create(UserEntity userEntity)
        {
            var mock = new Mock<IUserRepository>();
            UserEntity ue = new Fixture().Build<UserEntity>().Create();
            mock.Setup(s => s.GetByIdAsync(It.IsAny<string>(), string.Empty, null))
                .ReturnsAsync(ue);
            mock.Setup(s => s.ConfirmICR(It.IsAny<string>()))
                .Verifiable();
            mock.Setup(s => s.GetCurrentAsync(It.IsAny<string>()))
                .ReturnsAsync(ue);
            mock.Setup(s => s.ResetICR(It.IsAny<string>()))
                .Verifiable();
            mock.Setup(s => s.SaveAsync(It.IsAny<UserEntity>(), It.IsAny<string>()))
                .ReturnsAsync(userEntity);
            mock.Setup(s => s.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            return mock;
        }
    }
}
