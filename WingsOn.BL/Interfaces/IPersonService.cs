using System.Collections.Generic;
using WingsOn.Api.Resource;

namespace WingsOn.BL
{
    public interface IPersonService
    {
        IEnumerable<PersonResource> GetPeopleByCriteria(GenderTypeResource? gender, string flightNumber);

        PersonResource GetById(int id);

        void UpdatePerson(int personId, PersonResource person);
    }
}
