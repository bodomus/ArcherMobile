using Archer.AMA.BLL.Contracts;
using Archer.AMA.DTO;
using AutoFixture;
using Moq;

namespace ArcherMobilApp.BLL.Tests.Helpers
{
    public static class DocumentMocks
    {
        public static Mock<IDocumentService> DocumentServiceMoq()
        {
             var moq = new Mock<IDocumentService>(MockBehavior.Default);
             var d = new Fixture().Build<DocumentDTO>().Create();
             moq.Setup(s => s.GetByIdAsync(It.IsAny<int>(), null, null))
                 .ReturnsAsync(d);
            return moq;
        }
    
    }
}
