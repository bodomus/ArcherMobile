using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using Xunit;

using Archer.AMA.BLL.Contracts;
using Archer.AMA.DAL.Contract;
using Archer.AMA.DTO;
using ArcherMobilApp.BLL.Tests.Helpers;
using ArcherMobilApp.DAL.MsSql.Models;
using Newtonsoft.Json;

namespace ArcherMobilApp.BLL.Tests.ServicesTests
{
    public class UsersServiceTests
    {
        IUserRepository _userRepository;
        IMapper _mapper;

        public UsersServiceTests()
        {
            _mapper = Helpers.Mapper.GetAutoMapper();
        }

        [Fact(DisplayName = "Get user by id")]
        public async Task GetUserById_id2_ReturnUserDto()
        {
            IMapper mapper = Helpers.Mapper.GetAutoMapper();
            IUserService srv = new UserService(UserRepositoryMock.Create(null).Object, mapper);
            var ue = await srv.GetByIdAsync("2", String.Empty, null);

            Assert.NotNull(ue);
            Assert.IsAssignableFrom<UserDTO>(ue);
        }

        [Theory(DisplayName = "Create user.")]
        [InlineData("Name3", null, null)]
        public async Task CreateUser_Test_ReturnUser(string modelName, IUserRepository userRepository, IMapper mapper)
        {
            _mapper = Helpers.Mapper.GetAutoMapper();

            //Arrange
            var user = new Fixture().Build<UserEntity>().Create();

            //var usersRepoMoq = Moqs.UsersReposirotyMoq(mapper.Map<ComplexUserEntity>(user));
            var userSvc = new UserService(UserRepositoryMock.Create(user).Object, _mapper);

            //Act
            var newUser = await userSvc.SaveAsync(_mapper.Map<UserDTO>(user), string.Empty);

            ////Assert
            var actual = JsonConvert.SerializeObject(user);
            var expected = JsonConvert.SerializeObject(newUser);
            Assert.Equal(expected.Trim(), actual.Trim());
        }

        [Theory(DisplayName = "Update user.")]
        [InlineData("Name test", null, null)]
        public async Task UpdateUser_Test_ReturnUser(string modelName, IUserRepository userRepository, IMapper mapper)
        {
            //Arrange
            var user = new Fixture().Build<UserEntity>().Create();

            //var usersRepoMoq = Moqs.UsersReposirotyMoq(mapper.Map<ComplexUserEntity>(user));
            var userSvc = new UserService(UserRepositoryMock.Create(user).Object, _mapper);

            //Act
            var newUser = await userSvc.SaveAsync(_mapper.Map<UserDTO>(user), string.Empty);

            ////Assert
            Assert.NotNull(user);
            Assert.IsAssignableFrom<UserDTO>(newUser);
        }

        [Fact(DisplayName = "Delete user by id")]
        public async Task DeleteUserById_id2_ReturnTrue()
        {
            IUserService srv = new UserService(UserRepositoryMock.Create(null).Object, _mapper);
            var result = await srv.DeleteAsync("2", String.Empty);

            Assert.True(result);
        }

        [Fact(DisplayName = "Test get user by ID")]
        public async Task GetUser_Test_ReturnUserDTO()
        {
            //Arrange
            var user = new Fixture().Build<UserEntity>().Create();

            //var usersRepoMoq = Moqs.UsersReposirotyMoq(mapper.Map<ComplexUserEntity>(user));
            var userSvc = new UserService(UserRepositoryMock.Create(user).Object, _mapper);

            //Act
            var result = await userSvc.GetByIdAsync(Guid.NewGuid().ToString(), string.Empty, null);

            ////Assert
            Assert.IsAssignableFrom<UserDTO>(result);
            Assert.NotNull(result);
        }
    }
}
