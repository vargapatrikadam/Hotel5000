using AutoMapper;
using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                .ForMember(dest => dest.IdentityNumber, o => o.MapFrom(src => src.ApprovingData.IdentityNumber))
                .ForMember(dest => dest.TaxNumber, o => o.MapFrom(src => src.ApprovingData.TaxNumber))
                .ForMember(dest => dest.RegistrationNumber, o => o.MapFrom(src => src.ApprovingData.RegistrationNumber))
                //.ForMember(dest => dest.Contacts, o => o.MapFrom(src => src.Contacts))
                .ForMember(dest => dest.Contacts, o => o.Ignore());


            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Username, o => o.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, o => o.MapFrom(src => src.Password))
                .ForMember(dest => dest.Email, o => o.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, o => o.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, o => o.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Role, o => o.Ignore())
                .ForMember(dest => dest.ApprovingData, o => o.Ignore())
                .ForMember(dest => dest.Contacts, o => o.Ignore())
                .ForMember(dest => dest.Role, o => o.Ignore())
                .ForMember(dest => dest.Tokens, o => o.Ignore());

        }
    }
}
