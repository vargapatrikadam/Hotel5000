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

            CreateMap<Lodging, LodgingDto>()
                .ForMember(dest => dest.LodgingType, o => o.MapFrom(src => src.LodgingType.Name));
            CreateMap<LodgingDto, Lodging>()
                .ForMember(dest => dest.LodgingType, o => o.Ignore())
                .ForMember(dest => dest.Rooms, o => o.MapFrom(src => src.Rooms))
                .ForMember(dest => dest.LodgingAddresses, o => o.MapFrom(src =>src.LodgingAddresses));

            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.Currency, o => o.MapFrom(src => src.Currency.Name));
            CreateMap<RoomDto, Room>()
                .ForPath(dest => dest.Currency.Name, o => o.MapFrom(src => src.Currency));

            CreateMap<LodgingAddress, LodgingAddressDto>()
                .ForMember(dest => dest.CountryCode, o => o.MapFrom(src => src.Country.Code));
            CreateMap<LodgingAddressDto, LodgingAddress>()
                .ForPath(dest => dest.Country.Code, o => o.MapFrom(src => src.CountryCode));

            CreateMap<ReservationWindow, ReservationWindowDto>();
            CreateMap<ReservationWindowDto, ReservationWindow>();

            CreateMap<Currency, CurrencyDto>();
            CreateMap<CurrencyDto, Currency>();

            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.PaymentType, o => o.MapFrom(src => src.PaymentType.Name));
            CreateMap<ReservationDto, Reservation>()
                .ForMember(dest => dest.PaymentType, o => o.Ignore());

            CreateMap<ReservationItem, ReservationItemDto>()
                .ForMember(dest => dest.Currency, o => o.MapFrom(src => src.Room.Currency.Name))
                .ForMember(dest => dest.LodgingName, o => o.MapFrom(src => src.Room.Lodging.Name))
                .ForMember(dest => dest.Price, o => o.MapFrom(src => src.Room.Price));
            CreateMap<ReservationItemDto, ReservationItem>();
        }
    }
}
