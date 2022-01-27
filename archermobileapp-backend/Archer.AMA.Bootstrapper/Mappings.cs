using AutoMapper;

using Archer.AMA.DTO;
using Archer.AMA.Entity;
using ArcherMobilApp.DAL.MsSql.Models;
namespace Archer.AMA.Bootstrapper
{
    internal class Mappings : Profile
    {

        public Mappings()
        {
            Configure();
        }
        void Configure()
        {
            CreateMap<UserEntity, UserDTO>().ReverseMap();
            CreateMap<RoomEntity, RoomDTO>().ReverseMap();
            CreateMap<AnnouncementEntity, AnnouncementDTO>().ReverseMap();
            CreateMap<DocumentEntity, DocumentDTO>().ReverseMap();
            CreateMap<JobOpportunityEntity, JobOpportunityDTO>().ReverseMap();
        }
    }
}
