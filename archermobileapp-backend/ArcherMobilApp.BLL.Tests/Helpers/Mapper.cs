using AutoMapper;

namespace ArcherMobilApp.BLL.Tests.Helpers
{
    public static class Mapper
    {
        public static IMapper GetAutoMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                //cfg.AddProfile(new UsersMapping());

            });
            var mapper = mockMapper.CreateMapper();
            return mapper;
        }
    }
}
