using System;
using System.Collections.Generic;

namespace WingsOn.Api.Resource
{
    public class BookingResource
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public FlightResource Flight { get; set; }

        public PersonResource Customer { get; set; }

        public IEnumerable<PersonResource> Passengers { get; set; }

        public DateTime DateBooking { get; set; }
    }
}
