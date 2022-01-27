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
    public class AnnouncementEntityFrameworkTests
    {
        [Fact(DisplayName = "Check add announcement to Entity Framework")]
        public async Task AddAnnouncement_ToDb_ReturnAnnouncementAsync()
        {
            //Arrange
            IAnnouncementRepository rep = new TestRepositoryFactory<AnnouncementRepository>().Create();
            var countAnnouncements = (await rep.AllAsync(null, null)).Count();
            AnnouncementEntity d = new Fixture()
                .Build<AnnouncementEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Title, "Title test")
                .With(s => s.Description, "Description test")
                .With(s => s.ShortDescription, "Short description test")
                .With(s => s.UserCreatetorId, Guid.NewGuid().ToString)
                .With(s => s.AnnouncmentTypeId, 1)
                .With(s => s.PublishDate, DateTime.Now)
                .With(s => s.Date, DateTime.Now)
                .Create();

            //Act
            var d1 = await rep.SaveAsync(d, null);

            //Assert
            Assert.Equal(0, countAnnouncements);
            var countAnnouncementsActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countAnnouncements + 1, countAnnouncementsActual);


            Assert.Single(rep.AllAsync(null, null).Result);
            Assert.Equal("Title test", d1.Title);
        }

        [Fact(DisplayName = "Check update announcement to Entity Framework")]
        public async Task UpdateAnnouncement_InDb_ReturnAnnouncementAsync()
        {
            //Arrange
            IAnnouncementRepository rep = new TestRepositoryFactory<AnnouncementRepository>().Create();
            var countAnnouncements = (await rep.AllAsync(null, null)).Count();
            AnnouncementEntity d = new Fixture()
                .Build<AnnouncementEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Title, "Title test")
                .With(s => s.Description, "Description test")
                .With(s => s.ShortDescription, "Short description test")
                .With(s => s.UserCreatetorId, Guid.NewGuid().ToString)
                .With(s => s.AnnouncmentTypeId, 1)
                .With(s => s.PublishDate, DateTime.Now)
                .With(s => s.Date, DateTime.Now)
                .Create();

            AnnouncementEntity updAnnouncement = new Fixture()
                .Build<AnnouncementEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Title, "New Title test")
                .With(s => s.Description, "Description test")
                .With(s => s.ShortDescription, "Short description test")
                .With(s => s.UserCreatetorId, Guid.NewGuid().ToString)
                .With(s => s.AnnouncmentTypeId, 2)
                .With(s => s.PublishDate, DateTime.Now)
                .With(s => s.Date, DateTime.Now)
                .Create();

            //Act
            var addedNewAnnouncements = await rep.SaveAsync(d, null);

            var upd = await rep.SaveAsync(updAnnouncement, null);


            //Assert
            Assert.Equal(0, countAnnouncements);
            Assert.IsAssignableFrom<AnnouncementEntity>(upd);

            var countAnnouncementActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countAnnouncements + 1, countAnnouncementActual);

            Assert.Equal("New Title test", upd.Title);
            Assert.Equal(2, upd.AnnouncmentTypeId);
        }

        [Fact(DisplayName = "Check remove announcement to Entity Framework")]
        public async Task RemoveAnnouncement_FromDb_ReturnBoolAsync()
        {
            //Arrange
            IAnnouncementRepository rep = new TestRepositoryFactory<AnnouncementRepository>().Create();
            var countAnnouncements = (await rep.AllAsync(null, null)).Count();
            AnnouncementEntity d = new Fixture()
                .Build<AnnouncementEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Title, "Title test")
                .With(s => s.Description, "Description test")
                .With(s => s.ShortDescription, "Short description test")
                .With(s => s.UserCreatetorId, Guid.NewGuid().ToString)
                .With(s => s.AnnouncmentTypeId, 1)
                .With(s => s.PublishDate, DateTime.Now)
                .With(s => s.Date, DateTime.Now)
                .Create();

            var d1 = await rep.SaveAsync(d, null);

            //Act
            var result = await rep.DeleteAsync(d1.Id, null);

            //Assert
            Assert.Equal(0, countAnnouncements);
            var countAnnouncementActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countAnnouncements, countAnnouncementActual);

            //Assert Repo
            Assert.True(result);
        }

        [Fact(DisplayName = "Check add the same announcement to Entity Framework")]
        public async Task AddThesameAnnouncement_ToDb_ReturnAnnouncementAsync()
        {
            //Arrange
            IAnnouncementRepository rep = new TestRepositoryFactory<AnnouncementRepository>().Create();
            var countAnnouncements = (await rep.AllAsync(null, null)).Count();
            AnnouncementEntity d = new Fixture()
                .Build<AnnouncementEntity>()
                .With(s => s.Id, (int?)null)
                .With(s => s.Title, "Title test")
                .With(s => s.Description, "Description test")
                .With(s => s.ShortDescription, "Short description test")
                .With(s => s.UserCreatetorId, Guid.NewGuid().ToString)
                .With(s => s.AnnouncmentTypeId, 1)
                .With(s => s.PublishDate, DateTime.Now)
                .With(s => s.Date, DateTime.Now)
                .Create();
            var d1 = await rep.SaveAsync(d, null);

            //Act
            var d2 = await rep.SaveAsync(d1, null);

            //Assert
            Assert.Equal(0, countAnnouncements);
            var countDocsActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countAnnouncements + 1, countDocsActual);
        }
    }
}
