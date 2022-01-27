using System;
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
    public class DocumentEntityFrameworkTests
    {
        [Fact(DisplayName = "Check add document to Entity Framework")]
        public async Task AddDocument_ToDb_ReturnDocumentAsync()
        {
            //Arrange
            IDocumentRepository rep = new TestRepositoryFactory<DocumentsRepository>().Create();
            var countDocs = (await rep.AllAsync(null, null)).Count();
            DocumentEntity d = new Fixture()
                .Build<DocumentEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Title, "Title test")
                .With(s => s.DocumentTypeId, 1)
                .With(s => s.Uri, @"https://google.com")
                .Create();

            //Act
            var d1 = await rep.SaveAsync(d, null);

            //Assert
            Assert.Equal(0, countDocs);
            var countDocsActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countDocs + 1, countDocsActual);


            Assert.Single(rep.AllAsync(null, null).Result);
            Assert.Equal("Title test", d1.Title);
        }

        [Fact(DisplayName = "Check update document to Entity Framework")]
        public async Task UpdateDocument_InDb_ReturnDocumentAsync()
        {
            //Arrange
            IDocumentRepository rep = new TestRepositoryFactory<DocumentsRepository>().Create();
            var countDocs = (await rep.AllAsync(null, null)).Count();
            DocumentEntity newDocument = new Fixture()
                .Build<DocumentEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Title, "Title test")
                .With(s => s.DocumentTypeId, 1)
                .With(s => s.Uri, @"https://google.com")
                .Create();

            DocumentEntity updDocument = new Fixture()
                .Build<DocumentEntity>()
                .With(s => s.Id, 1)
                .With(s => s.Title, "Title test updated")
                .With(s => s.DocumentTypeId, 2)
                .With(s => s.Uri, @"https://google1.com")
                .Create();

            //Act
            var addedNewDocument = await rep.SaveAsync(newDocument, null);

            var upd = await rep.SaveAsync(updDocument, null);


            //Assert
            Assert.Equal(0, countDocs);
            var countDocsActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countDocs + 1, countDocsActual);

            Assert.Equal("Title test updated", upd.Title);
            Assert.Equal(2, upd.DocumentTypeId);
            Assert.Equal(@"https://google1.com", upd.Uri);

        }

        [Fact(DisplayName = "Check remove document to Entity Framework")]
        public async Task RemoveDocument_FromDb_ReturnBoolAsync()
        {
            //Arrange
            IDocumentRepository rep = new TestRepositoryFactory<DocumentsRepository>().Create();
            var countDocs = (await rep.AllAsync(null, null)).Count();
            DocumentEntity d = new Fixture()
                .Build<DocumentEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Title, "Title test")
                .With(s => s.DocumentTypeId, 1)
                .With(s => s.Uri, @"https://google.com")
                .Create();

            var d1 = await rep.SaveAsync(d, null);

            //Act
            var result = await rep.DeleteAsync(d1.Id, null);

            //Assert
            Assert.Equal(0, countDocs);
            var countDocsActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countDocs, countDocsActual);

            //Assert Repo
            Assert.True(result);
        }

        [Fact(DisplayName = "Check add the same document to Entity Framework")]
        public async Task AddThesameDocument_ToDb_ReturnDocumentAsync()
        {
            //Arrange
            IDocumentRepository rep = new TestRepositoryFactory<DocumentsRepository>().Create();
            var countDocs = (await rep.AllAsync(null, null)).Count();
            DocumentEntity d = new Fixture()
                .Build<DocumentEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Title, "Title test")
                .With(s => s.DocumentTypeId, 1)
                .With(s => s.Uri, @"https://google.com")
                .Create();
            var d1 = await rep.SaveAsync(d, null);

            //Act
            var d2 = await rep.SaveAsync(d1, null);

            //Assert
            Assert.Equal(0, countDocs);
            var countDocsActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countDocs + 1, countDocsActual);
        }
    }
}
