using System.Linq;
using System.Collections.Generic;
using WingsOn.Dal;
using WingsOn.Domain;
using AutoMapper;
using WingsOn.Api.Resource;

namespace WingsOn.BL
{
    public class PersonService : IPersonService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Booking> _bookingRepository;

        public PersonService(IRepository<Person> personRepository,
            IRepository<Booking> bookingRepository,
            IMapper mapper)
        {
            _personRepository = personRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public PersonResource GetById(int id)
        {
            var person = _personRepository.Get(id);
            return _mapper.Map<Person, PersonResource>(person);
        }

        public IEnumerable<PersonResource> GetPeopleByCriteria(GenderTypeResource? gender, string flightNumber)
        {
            var genderCriteria = _mapper.Map<GenderTypeResource?, GenderType?>(gender);

            var people = _bookingRepository
            .GetAll()
            .Where(booking => string.IsNullOrEmpty(flightNumber) || booking.Flight.Number == flightNumber)
            .SelectMany(bookingFiltered => bookingFiltered.Passengers)
            .Distinct()
            .Where(person => genderCriteria == null || person.Gender == genderCriteria);

            return _mapper.Map<IEnumerable<Person>, IEnumerable<PersonResource>>(people);
        }

        public void UpdatePerson(int personId, PersonResource person)
        {
            var personToUpdate = _mapper.Map<PersonResource, Person>(person);
            _personRepository.Save(personToUpdate);
        }
    }
}
