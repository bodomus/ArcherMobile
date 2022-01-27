using Archer.AMA.BLL.Contracts;
using Archer.AMA.DTO;
using AutoFixture;
using Moq;

namespace ArcherMobilApp.BLL.Tests.Helpers { 
    public static class UserServiceMocks
    {
        public static Mock<IUserService> UsersServiceMoq()
        {
            var moq = new Mock<IUserService>(MockBehavior.Default);
            var u = new Fixture().Build<UserDTO>().Create();
            moq.Setup(s => s.GetByIdAsync("2", null, null))
                .ReturnsAsync(u);
            moq.Setup(s => s.GetByIdAsync("2", null, null))
                .ReturnsAsync(u);
            return moq;
        }
    }
}
