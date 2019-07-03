using System.Collections.Generic;
using WingsOn.Domain;
using WingsOn.Dal;
using System.Linq;
using AutoMapper;
using WingsOn.Api.Resource;

namespace WingsOn.BL
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<Flight> _flightRepository;
        private readonly IRepository<Person> _personRepository;

        public BookingService(IRepository<Booking> bookingRepository,
            IRepository<Flight> flightRepository,
            IRepository<Person> personRepository,
            IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public BookingResource GetById(int id)
        {
            var domainBooking = _bookingRepository.Get(id);
            return _mapper.Map<Booking, BookingResource>(domainBooking);
        }

        public BookingResource CreateBooking(BookingResource booking, int flightId, int passengerId)
        {
            var existingFlight = _flightRepository.Get(flightId);
            var existingPassenger = _personRepository.Get(passengerId);

            if (existingFlight == null || existingPassenger == null)
            {
                return null;
            }

            var newId = (_bookingRepository.GetAll().Max(b => b.Id)) + 1;

            var bookingToSave = _mapper.Map<BookingResource, Booking>(booking);

            bookingToSave.Flight = existingFlight;
            bookingToSave.Passengers = new List<Person> { existingPassenger };
            bookingToSave.Id = newId;

            _bookingRepository.Save(bookingToSave);

            var savedBooking = _bookingRepository.Get(newId);
            return _mapper.Map<Booking, BookingResource>(savedBooking);
        }
    }
}
