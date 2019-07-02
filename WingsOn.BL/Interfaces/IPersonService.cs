using System;
using System.Collections.Generic;
using System.Text;
using WingsOn.Dal;
using WingsOn.Domain;
using System.Linq;

namespace WingsOn.BL
{
    public interface IPersonService
    {
        IEnumerable<Person> GetPeopleByCriteria(GenderType? gender, string flightNumber);

        Person GetById(int id);

        void UpdatePerson(int personId, Person person);
    }
}
