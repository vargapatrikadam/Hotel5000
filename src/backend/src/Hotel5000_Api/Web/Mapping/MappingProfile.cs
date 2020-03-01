using AutoMapper;
using Core.Entities.LodgingEntities;
using Web.DTOs;

namespace Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Username, o => o.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, o => o.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, o => o.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, o => o.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Role, o => o.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id));


            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Id, o => o.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, o => o.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, o => o.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, o => o.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, o => o.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Role, o => o.Ignore())
                .ForMember(dest => dest.Tokens, o => o.Ignore());

            CreateMap<Contact, ContactDto>();

            CreateMap<ContactDto, Contact>();

            CreateMap<ApprovingData, ApprovingDataDto>();

            CreateMap<ApprovingDataDto, ApprovingData>();
        }
    }
}
