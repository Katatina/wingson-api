using WingsOn.Api.Resource;

namespace WingsOn.BL
{
    public interface IBookingService
    {
        BookingResource GetById(int id);

        BookingResource CreateBooking(BookingResource booking, int flightId, int passengerId);
    }
}
