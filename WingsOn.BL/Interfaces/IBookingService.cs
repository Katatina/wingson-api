using System;
using System.Collections.Generic;
using System.Text;
using WingsOn.Domain;

namespace WingsOn.BL
{
    public interface IBookingService
    {
        Booking GetById(int id);

        Booking CreateBooking(Booking booking, int flightId, int passengerId);
    }
}
