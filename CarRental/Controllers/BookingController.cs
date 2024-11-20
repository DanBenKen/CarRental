using CarRental.Models;
using CarRental.Models.ViewModels.Booking;
using CarRental.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class BookingController : Controller
    {
        private readonly Garage _garage;
        private readonly List<Car> _cars;
        private readonly List<Booking> _bookings;

        public BookingController(Garage garage)
        {
            _garage = garage;
            _cars = _garage.GenerateCars().ToList();
            _bookings = _garage.GenerateBookings().ToList();
        }

        [HttpGet]
        public IActionResult Create(int carId)
        {
            var userId = 1;

            var car = _cars.FirstOrDefault(c => c.CarId == carId);

            var createViewModel = new CreateViewModel
            {
                UserId = userId,
                CarId = car.CarId,
                Make = car.Make,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
            };

            return View(createViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateViewModel bookingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(bookingModel);
            }

            decimal totalPrice = _garage.CalculateTotalPrice(bookingModel.StartDate, bookingModel.EndDate, bookingModel.PricePerDay);

            var newBooking = new Booking
            {
                BookingId = _bookings.Max(b => b.BookingId) + 1,
                CarId = bookingModel.CarId,
                PricePerDay = bookingModel.PricePerDay,
                UserId = bookingModel.UserId,
                StartDate = bookingModel.StartDate,
                EndDate = bookingModel.EndDate,
                TotalPrice = totalPrice,
            };

            _bookings.Add(newBooking);

            var car = _cars.FirstOrDefault(c => c.CarId == bookingModel.CarId);
            if (car == null) return NotFound("Car not found.");

            car.Status = "Rented";

            TempData["SuccessMessage"] = "Booking was Successful!";

            return RedirectToAction("History");
        }

        [HttpGet]
        public IActionResult History()
        {
            var userId = 1;

            var userBookings = _bookings.Where(b => b.UserId == userId).Select(b =>
            {
                var car = _cars.SingleOrDefault(c => c.CarId == b.CarId);

                return new HistoryViewModel
                {
                    BookingId = b.BookingId,
                    Make = car.Make,
                    Model = car.Model,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    TotalPrice = b.TotalPrice,
                };
            }).ToList();

            return View(userBookings);
        }

        [HttpGet]
        public IActionResult Return(int bookingId)
        {
            var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound("Booking not found.");

            var car = _cars.SingleOrDefault(c => c.CarId == booking.CarId);
            if (car == null) return NotFound("Car not found.");

            var returnViewModel = new ReturnViewModel
            {
                BookingId = booking.BookingId,
                Make = car.Make,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
            };

            return View(returnViewModel);
        }

        [HttpPost]
        public IActionResult Return(ReturnViewModel bookingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(bookingModel);
            }

            var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingModel.BookingId);
            if (booking == null) return NotFound("Booking not found.");

            var car = _cars.SingleOrDefault(c => c.CarId == booking.CarId);
            if (car == null) return NotFound("Car not found.");

            _bookings.Remove(booking);

            car.Status = "Available";

            return RedirectToAction("History");
        }
    }
}
