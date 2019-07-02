using System;
using Xunit;
using WingsOn.Api.Controllers;
using Moq;
using WingsOn.BL;
using WingsOn.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WingsOn.Api.Test
{
    public class BookingControllerTest
    {
        [Fact]
        public void GetBookingByIdSuccess()
        {
            var fakeBookingId = 1;
            var fakeBooking = new Booking { Id = 1, Number = "KSJHD" };

            var mockedBookingService = new Mock<IBookingService>();
            mockedBookingService.Setup(service => service.GetById(fakeBookingId)).Returns(fakeBooking);

            var bookingController = new BookingController(mockedBookingService.Object);

            var resultObject = bookingController.Get(fakeBookingId).Value;

            Assert.Same(resultObject, fakeBooking);
        }

        [Fact]
        public void GetBookingByIdNotFound()
        {
            var fakeBookingId = 1;

            var mockedBookingService = new Mock<IBookingService>();
            mockedBookingService.Setup(service => service.GetById(fakeBookingId)).Returns((Booking)null);

            var bookingController = new BookingController(mockedBookingService.Object);

            var resultObject = bookingController.Get(fakeBookingId).Result;

            Assert.IsType<NotFoundResult>(resultObject);
        }

        [Fact]
        public void CreateBookingBadRequest()
        {
            var fakeBooking = new Booking
            {
                Number = "sdfsf"
            };

            var mockedBookingService = new Mock<IBookingService>();
            mockedBookingService
                .Setup(service => service.CreateBooking(fakeBooking, 1, 1))
                .Returns((Booking)null);

            var bookingController = new BookingController(mockedBookingService.Object);

            var actionResult = bookingController.Post(fakeBooking, null, null);

            Assert.IsType<BadRequestResult>(actionResult);
        }

        [Fact]
        public void CreateBookingSuccess()
        {
            var fakeBooking = new Booking
            {
                Number = "sdfsf"
            };

            var mockedBookingService = new Mock<IBookingService>();
            mockedBookingService
                .Setup(service => service.CreateBooking(fakeBooking, 1, 1))
                .Returns(new Booking());

            var bookingController = new BookingController(mockedBookingService.Object);

            var actionResult = bookingController.Post(fakeBooking, 1, 1);

            Assert.IsType<CreatedAtActionResult>(actionResult);
        }
    }
}
