using Archer.AMA.Entity;
using ArcherMobilApp.BLL.Models;
using ArcherMobilApp.DAL.MsSql.Models;
using ArcherMobilApp.Models;
using AutoMapper;

namespace ArcherMobilApp.Infrastracture
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DocumentEntity, DocumentViewModel>().ReverseMap();

            //CreateMap<DocumentEntity, DocumentViewModel>().ForMember(a => a.Description, opt => opt.MapFrom(src => src.Description));
            //CreateMap<DocumentEntity, DocumentViewModel>().ForMember(a => a.DocumentTypeId, opt => opt.MapFrom(src => src.DocumentTypeId));
            //CreateMap<DocumentEntity, DocumentViewModel>().ForMember(a => a.Title, opt => opt.MapFrom(src => src.Title));
            //CreateMap<DocumentEntity, DocumentViewModel>().ForMember(a => a.Uri, opt => opt.MapFrom(src => src.Uri));
            //CreateMap<DocumentEntity, DocumentViewModel>().ForMember(a => a.Id, opt => opt.MapFrom(src => src.Id));



            //CreateMap<DocumentViewModel, DocumentEntity>().ForMember(a => a.Description, opt => opt.MapFrom(src => src.Description));
            //CreateMap<DocumentViewModel, DocumentEntity>().ForMember(a => a.DocumentTypeId, opt => opt.MapFrom(src => src.DocumentTypeId));
            //CreateMap<DocumentViewModel, DocumentEntity>().ForMember(a => a.Title, opt => opt.MapFrom(src => src.Title));
            //CreateMap<DocumentViewModel, DocumentEntity>().ForMember(a => a.Uri, opt => opt.MapFrom(src => src.Uri));
            //CreateMap<DocumentViewModel, DocumentEntity>().ForMember(a => a.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
