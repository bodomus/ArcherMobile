using Archer.AMA.BLL.Contracts;
using Archer.AMA.DTO;
using AutoFixture;
using Moq;

namespace ArcherMobilApp.BLL.Tests.Helpers
{
    public static class AnnouncementMock
    {
        public static Mock<IAnnouncementService> AnnouncementServiceMoq()
        {
             var moq = new Mock<IAnnouncementService>(MockBehavior.Default);
             var d = new Fixture().Build<AnnouncementDTO>().Create();
             moq.Setup(s => s.GetByIdAsync(It.IsAny<int>(), null, null))
                 .ReturnsAsync(d);
            return moq;
        }
    
    }
}
