using System;
using AutoMapper;
using ArcherMobilApp.BLL.Models;
using ArcherMobilApp.DAL.MsSql.Models;

namespace ArcherMobilApp.BLL.Mappings
{
    public class UsersMapping
    {
        public UsersMapping()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, UserEntity>();
            });

            IMapper mapper = config.CreateMapper();
            var source = new User();
            var dest = mapper.Map<User, UserEntity>(source);


            //AutoMapper.Mapper.CreateMap<User, ComplexUserEntity>()
            //    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.UserId))
            //    .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.UserName))
               
            //    .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
            //    .ForMember(d => d.ConfirmICR, opt => opt.MapFrom(src => src.ConfirmICR));

            //CreateMap<ComplexUserEntity, User>()
            //    .ForMember(d => d.UserId, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.UserName))
            //    .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
            //    .ForMember(d => d.ConfirmICR, opt => opt.MapFrom(src => src.ConfirmICR));
        }
    }
}
