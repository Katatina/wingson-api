using System;

namespace WingsOn.Api.Resource
{
    public class FlightResource
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public decimal Price { get; set; }
    }
}
