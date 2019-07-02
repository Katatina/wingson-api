using System.Collections.Generic;
using WingsOn.Domain;
using WingsOn.Dal;
using System.Linq;

namespace WingsOn.BL
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<Flight> _flightRepository;
        private readonly IRepository<Person> _personRepository;

        public BookingService(IRepository<Booking> bookingRepository,
            IRepository<Flight> flightRepository,
            IRepository<Person> personRepository)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _personRepository = personRepository;
        }

        public Booking GetById(int id)
        {
            return _bookingRepository.Get(id);
        }

        public Booking CreateBooking(Booking booking, int flightId, int passengerId)
        {
            var existingFlight = _flightRepository.Get(flightId);
            var existingPassenger = _personRepository.Get(passengerId);

            if (existingFlight == null || existingPassenger == null)
            {
                return null;
            }

            var newId = (_bookingRepository.GetAll().Max(b => b.Id)) + 1;

            booking.Flight = existingFlight;
            booking.Passengers = new List<Person> { existingPassenger };
            booking.Id = newId;

            _bookingRepository.Save(booking);

            return _bookingRepository.Get(newId);
        }
    }
}
