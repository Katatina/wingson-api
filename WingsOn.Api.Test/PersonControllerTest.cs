using System;
using Xunit;
using WingsOn.Api.Controllers;
using Moq;
using WingsOn.BL;
using WingsOn.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WingsOn.Api.Test
{
    public class PersonControllerTest
    {
        [Fact]
        public void GetPersonByIdSuccess()
        {
            var fakePerson = new Person { Id = 1, Name = "Katsiaryna", Address = "Minsk", Gender = GenderType.Female };

            var mockedPersonService = new Mock<IPersonService>();
            mockedPersonService.Setup(service => service.GetById(fakePerson.Id)).Returns(fakePerson);

            var personController = new PersonController(mockedPersonService.Object);

            var actionResult = personController.Get(fakePerson.Id);

            Assert.Same(actionResult.Value, fakePerson);
        }

        [Fact]
        public void GetPersonByIdNotFound()
        {
            var fakePersonId = 1;

            var mockedPersonService = new Mock<IPersonService>();
            mockedPersonService.Setup(service => service.GetById(fakePersonId)).Returns((Person)null);

            var personController = new PersonController(mockedPersonService.Object);

            var resultObject = personController.Get(fakePersonId).Result;

            Assert.IsType<NotFoundResult>(resultObject);
        }

        [Fact]
        public void GetPeopleByCriteriaSuccess()
        {
            var mockedPersonService = new Mock<IPersonService>();
            mockedPersonService
                .Setup(service => service.GetPeopleByCriteria(GenderType.Male, null))
                .Returns(GetMalePeople());

            var personController = new PersonController(mockedPersonService.Object);

            var actionResult = personController.Get(GenderType.Male, null).Result;

            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public void GetPeopleByCriteriaNotFound()
        {
            var mockedPersonService = new Mock<IPersonService>();
            mockedPersonService
                .Setup(service => service.GetPeopleByCriteria(GenderType.Male, null))
                .Returns(GetMalePeople());

            var personController = new PersonController(mockedPersonService.Object);

            var actionResult = personController.Get(GenderType.Female, null).Result;

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public void UpdatePersonSuccess()
        {
            var fakePerson = new Person { Id = 1};

            var mockedPersonService = new Mock<IPersonService>();
            mockedPersonService
                .Setup(service => service.UpdatePerson(1, fakePerson));

            var personController = new PersonController(mockedPersonService.Object);
            var actionResult = personController.Put(1, fakePerson);

            Assert.IsType<NoContentResult>(actionResult);
        }

        [Fact]
        public void UpdatePersonBadRequest()
        {
            var fakePerson = new Person { Id = 2 };

            var mockedPersonService = new Mock<IPersonService>();
            mockedPersonService
                .Setup(service => service.UpdatePerson(1, fakePerson));

            var personController = new PersonController(mockedPersonService.Object);

            var actionResult = personController.Put(1, fakePerson);

            Assert.IsType<BadRequestResult>(actionResult);
        }

        private IList<Person> GetMalePeople()
        {
            return new List<Person> {
                new Person
                {
                    Id= 1,
                    Gender = GenderType.Male,
                    Name = "John"
                },
                new Person
                {
                    Id= 2,
                    Gender = GenderType.Male,
                    Name = "Jack"
                }
            };
        }
    }
}
