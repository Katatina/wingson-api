using System.Linq;
using System.Collections.Generic;
using WingsOn.Dal;
using WingsOn.Domain;

namespace WingsOn.BL
{
    public class PersonService : IPersonService
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Booking> _bookingRepository;

        public PersonService(IRepository<Person> personRepository,
            IRepository<Booking> bookingRepository)
        {
            _personRepository = personRepository;
            _bookingRepository = bookingRepository;
        }

        public Person GetById(int id)
        {
            return _personRepository.Get(id);
        }

        public IEnumerable<Person> GetPeopleByCriteria(GenderType? gender, string flightNumber)
        {
            return _bookingRepository
            .GetAll()
            .Where(booking => string.IsNullOrEmpty(flightNumber) || booking.Flight.Number == flightNumber)
            .SelectMany(bookingFiltered => bookingFiltered.Passengers)
            .Distinct()
            .Where(person => gender == null || person.Gender == gender);
        }

        public void UpdatePerson(int personId, Person person)
        {
            _personRepository.Save(person);
        }
    }
}
