using System;
using Xunit;
using WingsOn.Api.Controllers;
using Moq;
using WingsOn.BL;
using WingsOn.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WingsOn.Api.Resource;

namespace WingsOn.Api.Test
{
    public class PersonControllerTest
    {
        [Fact]
        public void GetPersonByIdSuccess()
        {
            var fakePerson = new PersonResource { Id = 1, Name = "Katsiaryna", Address = "Minsk", Gender = GenderTypeResource.Female };

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
            mockedPersonService.Setup(service => service.GetById(fakePersonId)).Returns((PersonResource)null);

            var personController = new PersonController(mockedPersonService.Object);

            var resultObject = personController.Get(fakePersonId).Result;

            Assert.IsType<NotFoundResult>(resultObject);
        }

        [Fact]
        public void GetPeopleByCriteriaSuccess()
        {
            var mockedPersonService = new Mock<IPersonService>();
            mockedPersonService
                .Setup(service => service.GetPeopleByCriteria(GenderTypeResource.Male, null))
                .Returns(GetMalePeople());

            var personController = new PersonController(mockedPersonService.Object);

            var actionResult = personController.Get(GenderTypeResource.Male, null).Result;

            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public void GetPeopleByCriteriaNotFound()
        {
            var mockedPersonService = new Mock<IPersonService>();
            mockedPersonService
                .Setup(service => service.GetPeopleByCriteria(GenderTypeResource.Male, null))
                .Returns(GetMalePeople());

            var personController = new PersonController(mockedPersonService.Object);

            var actionResult = personController.Get(GenderTypeResource.Female, null).Result;

            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public void UpdatePersonSuccess()
        {
            var fakePerson = new PersonResource { Id = 1};

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
            var fakePerson = new PersonResource { Id = 2 };

            var mockedPersonService = new Mock<IPersonService>();
            mockedPersonService
                .Setup(service => service.UpdatePerson(1, fakePerson));

            var personController = new PersonController(mockedPersonService.Object);

            var actionResult = personController.Put(1, fakePerson);

            Assert.IsType<BadRequestResult>(actionResult);
        }

        private IList<PersonResource> GetMalePeople()
        {
            return new List<PersonResource> {
                new PersonResource
                {
                    Id= 1,
                    Gender = GenderTypeResource.Male,
                    Name = "John"
                },
                new PersonResource
                {
                    Id= 2,
                    Gender = GenderTypeResource.Male,
                    Name = "Jack"
                }
            };
        }
    }
}
