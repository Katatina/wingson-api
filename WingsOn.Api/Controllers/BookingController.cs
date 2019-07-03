using Microsoft.AspNetCore.Mvc;
using WingsOn.Api.Resource;
using WingsOn.BL;

namespace WingsOn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{id}")]
        public ActionResult<BookingResource> Get(int id)
        {
            var booking = 
                _bookingService.GetById(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpPost]
        public ActionResult Post([FromBody]BookingResource booking, [FromQuery]int? flightId, [FromQuery]int? passengerId)
        {
            if (booking == null || flightId == null || passengerId == null)
            {
                return BadRequest();
            }

            var createdBooking = _bookingService.CreateBooking(booking, (int)flightId, (int)passengerId);
            return CreatedAtAction("Get", new { id = createdBooking.Id }, createdBooking);
        }
    }
}