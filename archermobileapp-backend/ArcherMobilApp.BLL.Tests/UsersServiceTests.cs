using System;

using AutoFixture;
using Newtonsoft.Json;
using Xunit;

using ArcherMobilApp.BLL.Tests.Helpers;
using ArcherMobilApp.DAL.MsSql.Models;

namespace ArcherMobilApp.BLL.Tests
{
    public class UsersServiceTests
    {
        [Theory]
        [InlineData("Name1")]
        [InlineData("Name2")]
        [InlineData("Name3")]
        public void CreateUser_Test(string modelName)
        {
            //Arrange
            var fixture = new Fixture();

            var user = Fixtures.UserFixture(modelName);
            var mapper = Mapper.GetAutoMapper();
            var usersRepoMoq = Moqs.UsersReposirotyMoq(mapper.Map<ComplexUserEntity>(user));
            var userSvc = new UsersService(mapper, usersRepoMoq.Object);

            string guid = Guid.NewGuid().ToString();
            //Act
            var newUser = userSvc.CreateUserAsync(guid, user).Result;

            //Assert
            var actual = JsonConvert.SerializeObject(user);
            var expected = JsonConvert.SerializeObject(newUser);
            Assert.Equal(expected.Trim(), actual.Trim());
        }

        [Fact]
        public void GetUser_Test()
        {
            //Arrange
            var fixture = new Fixture();

            var user = Fixtures.UserFixture();
            var mapper = Mapper.GetAutoMapper();
            var usersRepoMoq = Moqs.UsersReposirotyMoq(mapper.Map<ComplexUserEntity>(user));
            var userSvc = new UsersService(mapper, usersRepoMoq.Object);

            //Act
            var result = userSvc.GetUserAsync(fixture.Create<string>()).Result;

            //Assert
            var actual = JsonConvert.SerializeObject(user);
            var expected = JsonConvert.SerializeObject(result);
            Assert.Equal(expected.Trim(), actual.Trim());
        }
    }
}
