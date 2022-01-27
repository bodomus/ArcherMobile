//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Archer.AMA.DAL.Contract;
//using Archer.AMA.DTO;
//using Archer.AMA.Entity;
//using ArcherMobilApp.BLL.Tests.Helpers;

//using ArcherMobilApp.DAL.MsSql;
//using AutoFixture;
//using Moq;
//using Newtonsoft.Json;
//using Xunit;

//namespace ArcherMobilApp.BLL.Tests.ServicesTests
//{
//    public class DocumentServiceTests
//    {
//        [Theory(DisplayName = "Check create document")]
//        [InlineData("New document1")]
//        public void CreateDocumentReturnDocument(string documentName)
//        {
//            //Arrange
//            var mapper = Helpers.Mapper.GetAutoMapper();
//            var document = Fixtures.DocumentDTOFixture(documentName);
//            var userId = new Fixture().Create<Guid>().ToString();
//            var docRepoMoq = Moqs.DocumentRepositoryMoq(mapper.Map<DocumentEntity>(document), userId);
//            var docSvc = new DocumentService(docRepoMoq.Object, mapper);

//            //Act
//            var newDoc = docSvc.SaveAsync(document, userId).Result;

//            ////Assert
//            var actual = JsonConvert.SerializeObject(document);
//            var expected = JsonConvert.SerializeObject(newDoc);
//            docRepoMoq.Verify();
//            Assert.Equal(expected.Trim(), actual.Trim());
//        }

//        [Theory(DisplayName = "Check delete document")]
//        [InlineData("Document", 2)]
//        public void DeleteDocument_TrueReturned(string documentName, int id)
//        {
//            // "123D9C71-25CF-4CFB-ABE8-CCE7FE12969D"
//            //Arrange
//            var mapper = Helpers.Mapper.GetAutoMapper();
//            var document = Fixtures.DocumentDTOFixture(documentName);
//            var user = new Fixture().Create<Guid>().ToString();

//            var docRepoMoq = Moqs.DocumentRepositoryMoq(mapper.Map<DocumentEntity>(document), user);
//            var docSvc = new DocumentService(docRepoMoq.Object, mapper);
//            //Act
//            var result = docSvc.DeleteAsync(id, user).Result;

//            ////Assert
//            Assert.True(result);
//            docRepoMoq.Verify();
//        }

//        [Theory(DisplayName = "Check update document")]
//        [InlineData("New document update 1", 1)]
//        public void UpdateDocument_ReturnDocument(string documentName, int? id)
//        {
//            //Arrange
//            var mapper = Helpers.Mapper.GetAutoMapper();

//            var document = Fixtures.DocumentDTOFixture(documentName, id);

//            var user = new Fixture().Create<Guid>().ToString();

//            var docRepoMoq = Moqs.DocumentRepositoryMoq(mapper.Map<DocumentEntity>(document), user);
//            var docSvc = new DocumentService(docRepoMoq.Object, mapper);

//            //Act
//            var newDoc = docSvc.SaveAsync(document, user).Result;

//            ////Assert
//            var actual = JsonConvert.SerializeObject(document);
//            var expected = JsonConvert.SerializeObject(newDoc);
//            Assert.Equal(expected.Trim(), actual.Trim());
//            docRepoMoq.Verify();
//        }

//        [Fact(DisplayName = "Check get 20 documents", Skip = "Need check Mock")]
//        public void GetDocuments_Return20Documents()
//        {
//            //Arrange
//            var mapper = Helpers.Mapper.GetAutoMapper();

//            var document = Fixtures.DocumentDTOFixture(string.Empty, null);

//            var user = new Fixture().Create<Guid>().ToString();

//            var docRepoMoq = Moqs.DocumentRepositoryMoq(mapper.Map<DocumentEntity>(document), user);
//            var docSvc = new DocumentService(docRepoMoq.Object, mapper);

//            //Act
//            var docs = docSvc.AllAsync(user).Result;

//            ////Assert

//            docRepoMoq.Verify(x => x.AllAsync("", null), Times.Exactly(20));

//            Assert.Equal(20, docs.Count());
//            Assert.IsAssignableFrom<IEnumerable<DocumentDTO>>(docs);
//        }

//        [Theory(DisplayName = "Check get document")]
//        [InlineData(2)]
//        public void GetDocument_ById2_ReturnDocumentAsync(int id)
//        {
//            //Arrange
//            var fixture = new Fixture();
//            var model = fixture.Create<DocumentEntity>();
//            var fixture1 = new Fixture();
//            var model1 =
//                fixture1
//                    .Build<DocumentEntity>()
//                    .Without(x => x.Id)
//                    .Create();

//            var mapper = Helpers.Mapper.GetAutoMapper();

//            var document = Fixtures.DocumentDTOFixture(string.Empty, null);

//            var user = new Fixture().Create<Guid>().ToString();

//            var docRepoMoq = Moqs.DocumentRepositoryMoq(mapper.Map<DocumentEntity>(document), user);
//            var docSvc = new DocumentService(docRepoMoq.Object, mapper);

//            //Act
//            var doc = docSvc.GetByIdAsync(id, user, null).Result;

//            ////Assert
//            Assert.IsType<DocumentDTO>(doc);
//            docRepoMoq.Verify();
//        }
//    }
//}
