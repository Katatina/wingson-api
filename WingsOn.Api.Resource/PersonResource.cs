using System;

namespace WingsOn.Api.Resource
{
    public class PersonResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateBirth { get; set; }

        public GenderTypeResource Gender { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
    }
}
