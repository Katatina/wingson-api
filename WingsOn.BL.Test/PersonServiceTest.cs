using System;
using Xunit;
using Moq;
using WingsOn.BL;
using WingsOn.Dal;
using WingsOn.Domain;
using System.Collections.Generic;
using AutoMapper;
using WingsOn.Api.Resource;
using System.Globalization;
using System.Linq;

namespace WingsOn.BL.Test
{
    public class PersonServiceTest
    {
        [Fact]
        public void GetById()
        {
            var fakePerson = new Person { Id = 1, Name = "Kate" };

            var mockedMapper = GetMockedMapper();
            var mockedPersonRepository = GetMockedPersonRepository();
            var mockedBookingRepository = GetMockedBookingRepository();

            var personService = new PersonService(mockedPersonRepository.Object, mockedBookingRepository.Object, mockedMapper.Object);

            var result = personService.GetById(fakePerson.Id);

            Assert.IsType<PersonResource>(result);
            Assert.Equal(fakePerson.Name, result.Name);
        }

        [Fact]
        public void GetPeopleByCriteriaByGender()
        {
            var fakeGenderSearchCriteria = GenderTypeResource.Male;

            var mockedMapper = GetMockedMapper();
            var mockedBookingRepository = GetMockedBookingRepository();
            var mockedPersonRepository = GetMockedPersonRepository();

            var personService = new PersonService(mockedPersonRepository.Object, mockedBookingRepository.Object, mockedMapper.Object);

            var result = personService.GetPeopleByCriteria(fakeGenderSearchCriteria, null);

            Assert.All(result, item => Assert.Equal(fakeGenderSearchCriteria, item.Gender));
        }

        [Fact]
        public void UpdatePerson()
        {
            var mockedMapper = GetMockedMapper();
            var mockedBookingRepository = GetMockedBookingRepository();
            var mockedPersonRepository = GetMockedPersonRepository();

            var personService = new PersonService(mockedPersonRepository.Object, mockedBookingRepository.Object, mockedMapper.Object);

            var fakePerson = new PersonResource { Id = 14, Name = "John Doe" };

            personService.UpdatePerson(fakePerson.Id, fakePerson);

            mockedPersonRepository.Verify(m => m.Save(It.IsAny<Person>()), Times.Once());
        }

        private List<Booking> GetFakeBookings()
        {
            CultureInfo cultureInfo = new CultureInfo("nl-NL");

            var fakeBookings = new List<Booking>
            {
                new Booking
                {
                    Id = 83,
                    Number = "WO-151277",
                    Customer = new Person{ Gender = GenderType.Female, Name = "Debra Lang"},
                    DateBooking = DateTime.Parse("12/02/2000 12:55", cultureInfo),
                    Flight = new Flight { Number="PZ696" },
                    Passengers = new[]
                    {
                        new Person{ Gender = GenderType.Male, Name = "Claire Stephens"},
                        new Person{ Gender = GenderType.Female, Name = "Kendall Velazquez"},
                        new Person{ Gender = GenderType.Male, Name = "Zenia Stout"}
                    }
                },
                new Booking
                {
                    Id = 34,
                    Number = "WO-694142",
                    Customer = new Person{ Gender = GenderType.Female, Name = "Kathy Morgan"},
                    DateBooking = DateTime.Parse("13/02/2000 16:37", cultureInfo),
                    Flight = new Flight { Number="TEST" },
                    Passengers = new[]
                    {
                        new Person{ Gender = GenderType.Female, Name = "Kathy Morgan"}
                    }
                }
            };

            return fakeBookings;
        }

        private Mock<IMapper> GetMockedMapper()
        {
            var mockedMapper = new Mock<IMapper>();
            mockedMapper
                .Setup(x => x.Map<Person, PersonResource>(It.IsAny<Person>()))
                .Returns((Person source) => new PersonResource() { Name = source.Name });

            mockedMapper
                .Setup(x => x.Map<GenderTypeResource?, GenderType?>(GenderTypeResource.Male))
                .Returns((GenderTypeResource? source) =>
                {
                    return GenderType.Male;
                });

            mockedMapper
                .Setup(x => x
                    .Map<IEnumerable<Person>, IEnumerable<PersonResource>>(It.IsAny<IEnumerable<Person>>()))
                    .Returns((IEnumerable<Person> source) =>
                    {
                        return source.Select(item =>
                            new PersonResource
                            {
                                Name = item.Name,
                                Gender = item.Gender == GenderType.Male ? GenderTypeResource.Male : GenderTypeResource.Female,
                                Id = item.Id
                            });
                    });

            return mockedMapper;
        }

        private Mock<IRepository<Booking>> GetMockedBookingRepository()
        {
            var fakeBookings = GetFakeBookings();

            var mockedBookingRepository = new Mock<IRepository<Booking>>();
            mockedBookingRepository
                .Setup(repository => repository.GetAll())
                .Returns(fakeBookings);

            return mockedBookingRepository;
        }

        private Mock<IRepository<Person>> GetMockedPersonRepository()
        {
            var fakePerson = new Person { Id = 1, Name = "Kate" };

            var mockedPersonRepository = new Mock<IRepository<Person>>();
            mockedPersonRepository
                .Setup(repository => repository.Get(fakePerson.Id))
                .Returns(fakePerson);

            mockedPersonRepository
                .Setup(repository => repository.Save(It.IsAny<Person>()));

            return mockedPersonRepository;
        }
    }
}
