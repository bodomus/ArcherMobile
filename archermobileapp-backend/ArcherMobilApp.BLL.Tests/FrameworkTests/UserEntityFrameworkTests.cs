using System;
using System.Linq;
using System.Threading.Tasks;

using Archer.AMA.DAL.Contract;
using Archer.AMA.Entity;
using ArcherMobilApp.BLL.Tests.Helpers;
using ArcherMobilApp.DAL.MsSql;
using ArcherMobilApp.DAL.MsSql.Models;
using AutoFixture;
using Xunit;

namespace ArcherMobilApp.BLL.Tests.FrameworkTests
{
    public class UserEntityFrameworkTests
    {
        [Fact(DisplayName = "Check add user to Entity Framework")]
        public async Task AddUser_ToDb_ReturnUserAsync()
        {
            //Arrange
            IUserRepository rep = new TestRepositoryFactory<UserRepository>().Create();
            var countUsers = (await rep.AllAsync(null, null)).Count();
            UserEntity d = new Fixture()
                .Build<UserEntity>()
                .With(s => s.Id, string.Empty)
                .With(s => s.UserName, "User test name")
                .With(s => s.Email, @"test@gmail.com")
                .With(s => s.IsAdmin, false)
                .With(s => s.ConfirmICR, false)
                .With(s => s.ICRConfirmationDate, DateTime.Now)
                .With(s => s.LastLogin, DateTime.Now)
                .Create();

            //Act
            var d1 = await rep.SaveAsync(d, null);

            //Assert
            Assert.Equal(0, countUsers);
            var countUsersActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countUsers + 1, countUsersActual);

            Assert.Single(rep.AllAsync(null, null).Result);
            Assert.Equal(@"User test name", d1.UserName);
            Assert.False(d1.IsAdmin);
            Assert.False(d1.ConfirmICR);
        }

        [Fact(DisplayName = "Check remove user from Entity Framework")]
        public async Task RemoveUser_FromDb_ReturnBoolAsync()
        {
            //Arrange
            IUserRepository rep = new TestRepositoryFactory<UserRepository>().Create();
            var countUsers = (await rep.AllAsync(null, null)).Count();
            UserEntity newUser = new Fixture()
                .Build<UserEntity>()
                .With(s => s.Id, string.Empty)
                .With(s => s.UserName, "User test name")
                .With(s => s.Email, @"test@gmail.com")
                .With(s => s.IsAdmin, false)
                .With(s => s.ConfirmICR, false)
                .With(s => s.ICRConfirmationDate, DateTime.Now)
                .With(s => s.LastLogin, DateTime.Now)
                .Create();

            var d1 = await rep.SaveAsync(newUser, null);

            //Act
            var result = await rep.DeleteAsync(d1.Id, null);

            //Assert
            Assert.Equal(0, countUsers);
            var countUsersActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countUsers, countUsersActual);

            //Assert Repo
            Assert.True(result);
        }

        [Fact(DisplayName = "Check add the same user to Entity Framework")]
        public async Task AddThesameUser_ToDb_ReturnUserAsync()
        {
            //Arrange
            IUserRepository rep = new TestRepositoryFactory<UserRepository>().Create();
            var countUsers = (await rep.AllAsync(null, null)).Count();
            UserEntity newUser = new Fixture()
                .Build<UserEntity>()
                .With(s => s.Id, string.Empty)
                .With(s => s.UserName, "User test name")
                .With(s => s.Email, @"test@gmail.com")
                .With(s => s.IsAdmin, false)
                .With(s => s.ConfirmICR, false)
                .With(s => s.ICRConfirmationDate, DateTime.Now)
                .With(s => s.LastLogin, DateTime.Now)
                .Create();
            var d1 = await rep.SaveAsync(newUser, null);

            //Act
            var d2 = await rep.SaveAsync(d1, null);

            //Assert
            Assert.Equal(0, countUsers);
            var countRoomsActual = (await rep.AllAsync(null, null)).Count();
            Assert.Equal(countUsers + 1, countRoomsActual);
        }
    }
}

