﻿using AutoMapper;
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

            CreateMap<Lodging, LodgingDto>()
                .ForMember(dest => dest.LodgingType, o => o.MapFrom(src => src.LodgingType.Name));
            CreateMap<LodgingDto, Lodging>();

            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.Currency, o => o.MapFrom(src => src.Currency.Name));
            CreateMap<RoomDto, Room>();

            CreateMap<LodgingAddress, LodgingAddressDto>()
                .ForMember(dest => dest.CountryCode, o => o.MapFrom(src => src.Country.Code));
            CreateMap<LodgingAddressDto, LodgingAddress>();

            CreateMap<ReservationWindow, ReservationWindowDto>();
            CreateMap<ReservationWindowDto, ReservationWindow>();

            CreateMap<Currency, CurrencyDto>();
            CreateMap<CurrencyDto, Currency>();
        }
    }
}
