//using AutoFixture;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using NSubstitute;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace Admin.Chatbot.UnitTests.Application
//{
//    public class AuthAppServiceTest
//    {
//        private readonly UserManager<ApplicationUser> userManager;
//        private readonly SignInManager<ApplicationUser> signInManager;
//        private readonly Fixture fixture;
//        private readonly AuthSettings authSettings;
//        private readonly ApplicationUser applicationUser;
//        private readonly LoginInput loginInput;
//        private readonly IAuthAppService service;

//        private readonly ISettingRepository settingRepository;
//        private readonly Setting setting;

//        public AuthAppServiceTest()
//        {
//            fixture = new Fixture();

//            setting = new Setting
//            {
//                UserName = "username",
//                Password = "password"
//            };

//            settingRepository = Substitute.For<ISettingRepository>();
//            settingRepository.GetAsync().Returns(setting);

//            authSettings = fixture.Create<AuthSettings>();
//            loginInput = fixture.Create<LoginInput>();
//            applicationUser = fixture.Build<ApplicationUser>().With(x => x.Active, true).Create();

//            userManager = Substitute.For<UserManager<ApplicationUser>>(Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
//            userManager.FindByIdAsync(Arg.Any<string>()).Returns(applicationUser);

//            signInManager = Substitute.For<SignInManager<ApplicationUser>>
//                (
//                    userManager,
//                    Substitute.For<IHttpContextAccessor>(),
//                    Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null
//                );
//            signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(SignInResult.Success);

//            service = new AuthAppService(signInManager, userManager);
//        }

//        [Fact]
//        public async Task Should_Login_User()
//        {
//            var token = await service.LoginAsync(loginInput, authSettings.Key, authSettings.Expiration);

//            var validationParameters = new TokenValidationParameters
//            {
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key)),
//                ValidateAudience = false,
//                ValidateIssuer = false
//            };

//            var claims = new JwtSecurityTokenHandler().ValidateToken(token.Token, validationParameters, out SecurityToken _);

//            Assert.Equal(authSettings.Expiration, token.Expiration);
//            Assert.Equal(applicationUser.Profile.ToString(), claims.FindFirst(x => x.Type == ClaimTypes.Role).Value);

//            await userManager.Received().FindByIdAsync(loginInput.UserName);

//            await signInManager.Received().PasswordSignInAsync
//                (
//                    userName: loginInput.UserName,
//                    password: loginInput.Password,
//                    isPersistent: false,
//                    lockoutOnFailure: false
//                );
//        }

//        [Fact]
//        public async Task Not_Should_Login_User()
//        {
//            signInManager.PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(SignInResult.Failed);

//            await Assert.ThrowsAsync<AuthLoginException>(() => service.LoginAsync(loginInput, authSettings.Key, authSettings.Expiration));

//            await signInManager.Received().PasswordSignInAsync
//                (
//                    userName: loginInput.UserName,
//                    password: loginInput.Password,
//                    isPersistent: false,
//                    lockoutOnFailure: false
//                );
//        }

//        [Fact]
//        public async Task Not_Should_Login_Inactived_User()
//        {
//            applicationUser.Active = false;

//            userManager.FindByIdAsync(Arg.Any<string>()).Returns(applicationUser);

//            await Assert.ThrowsAsync<AuthLoginException>(() => service.LoginAsync(loginInput, authSettings.Key, authSettings.Expiration));

//            await signInManager.DidNotReceive().PasswordSignInAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>());
//            await userManager.Received().FindByIdAsync(loginInput.UserName);
//        }


//        [Fact]
//        public async Task Should_Refresh_Token()
//        {
//            var token = await service.RefreshTokenAsync(loginInput.UserName, authSettings.Key, authSettings.Expiration);

//            var validationParameters = new TokenValidationParameters
//            {
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key)),
//                ValidateAudience = false,
//                ValidateIssuer = false
//            };

//            var claims = new JwtSecurityTokenHandler().ValidateToken(token.Token, validationParameters, out SecurityToken _);

//            Assert.Equal(authSettings.Expiration, token.Expiration);
//            Assert.Equal(applicationUser.Profile.ToString(), claims.FindFirst(x => x.Type == ClaimTypes.Role).Value);

//            await userManager.Received().FindByIdAsync(loginInput.UserName);
//        }
//    }
//}