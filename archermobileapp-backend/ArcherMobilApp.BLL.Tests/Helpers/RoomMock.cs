using Archer.AMA.BLL.Contracts;
using Archer.AMA.DTO;
using AutoFixture;
using Moq;

namespace ArcherMobilApp.BLL.Tests.Helpers
{
    public static class RoomMocks
    {
        public static Mock<IRoomService> RoomServiceMoq()
        {
             var moq = new Mock<IRoomService>(MockBehavior.Default);
             var d = new Fixture().Build<RoomDTO>().Create();
             moq.Setup(s => s.GetByIdAsync(It.IsAny<int>(), null, null))
                 .ReturnsAsync(d);
            return moq;
        }
    }
}
