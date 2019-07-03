using AutoMapper;
using WingsOn.Api.Resource;
using WingsOn.Domain;

namespace WingsOn.Api.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonResource, Person>();
            CreateMap<Person, PersonResource>();

            CreateMap<BookingResource, Booking>();
            CreateMap<Booking, BookingResource>();

            CreateMap<FlightResource, Flight>();
            CreateMap<Flight, FlightResource>();

            CreateMap<GenderTypeResource, GenderType>();
            CreateMap<GenderType, GenderTypeResource>();
        }
    }
}
