using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WingsOn.Api.Resource;
using WingsOn.BL;

namespace WingsOn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PersonResource>> Get([FromQuery] GenderTypeResource? gender, [FromQuery] string flightNumber)
        {
            var people = _personService.GetPeopleByCriteria(gender, flightNumber);

            if (people == null || people.Count() < 1)
            {
                return NotFound();
            }

            return Ok(_personService.GetPeopleByCriteria(gender, flightNumber).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<PersonResource> Get(int id)
        {
            var person = _personService.GetById(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]PersonResource person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _personService.UpdatePerson(id, person);

            return NoContent();
        }
    }
}
