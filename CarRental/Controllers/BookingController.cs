using CarRental.Models.ViewModels.Booking;
using CarRental.Models;
using CarRental.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarRental.Controllers
{
    public class BookingController : Controller
    {
        private readonly Garage _garage;
        private readonly List<Car> _cars;
        private readonly List<Booking> _bookings;

        public BookingController(Garage garage, IHttpContextAccessor httpContextAccessor)
        {
            _garage = garage;

            _cars = _garage.GenerateCars()?.ToList();

            _bookings = LoadBookingsFromSession(httpContextAccessor);
        }

        [HttpGet]
        public IActionResult Create(int carId)
        {
            int userId = 1;

            var car = _cars.FirstOrDefault(c => c.CarId == carId);
            if (car == null) return NotFound("Car not found.");

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
            if (!ModelState.IsValid) return View(bookingModel);

            int userId = 1;

            var car = _cars.FirstOrDefault(c => c.CarId == bookingModel.CarId);
            if (car == null) return NotFound("Car not found.");

            var totalPrice = _garage.CalculateTotalPrice(bookingModel.StartDate, bookingModel.EndDate, bookingModel.PricePerDay);

            var newBooking = new Booking
            {
                BookingId = _bookings.Any() ? _bookings.Max(b => b.BookingId) + 1 : 1,
                CarId = bookingModel.CarId,
                UserId = userId,
                StartDate = bookingModel.StartDate,
                EndDate = bookingModel.EndDate,
                TotalPrice = totalPrice,
            };

            _bookings.Add(newBooking);
            SaveBookingsToSession();

            car.Status = "Rented";

            return RedirectToAction("Confirmation", new { bookingId = newBooking.BookingId });
        }

        [HttpGet]
        public IActionResult History()
        {
            var userId = 1;

            var userBookings = _bookings
                .Where(b => b.UserId == userId)
                .Select(b =>
                {
                    var car = _cars.FirstOrDefault(c => c.CarId == b.CarId);
                    return new HistoryViewModel
                    {
                        BookingId = b.BookingId,
                        Make = car?.Make,
                        Model = car?.Model,
                        StartDate = b.StartDate,
                        EndDate = b.EndDate,
                        TotalPrice = b.TotalPrice,
                    };
                }).ToList();

            return View(userBookings);
        }

        [HttpGet]
        public IActionResult Confirmation(int bookingId)
        {
            var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound("Booking not found.");

            var car = _cars.FirstOrDefault(c => c.CarId == booking.CarId);
            if (car == null) return NotFound("Car not found.");

            var confirmationViewModel = new ConfirmationViewModel
            {
                BookingId = booking.BookingId,
                Make = car.Make,
                Model = car.Model,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                TotalPrice = booking.TotalPrice,
            };

            return View(confirmationViewModel);
        }

        [HttpGet]
        public IActionResult Return(int bookingId)
        {
            var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound("Booking not found.");

            var car = _cars.FirstOrDefault(c => c.CarId == booking.CarId);
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
            if (!ModelState.IsValid) return View(bookingModel);

            var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingModel.BookingId);
            if (booking == null) return NotFound("Booking not found.");

            var car = _cars.FirstOrDefault(c => c.CarId == booking.CarId);
            if (car == null) return NotFound("Car not found.");

            _bookings.Remove(booking);
            SaveBookingsToSession();

            car.Status = "Available";

            return RedirectToAction("History");
        }

        private List<Booking>? LoadBookingsFromSession(IHttpContextAccessor httpContextAccessor)
        {
            var bookingsSession = httpContextAccessor.HttpContext?.Session?.GetString("Bookings");
            return string.IsNullOrEmpty(bookingsSession)
                ? _garage.GenerateBookings().ToList()
                : JsonConvert.DeserializeObject<List<Booking>>(bookingsSession);
        }

        private void SaveBookingsToSession()
        {
            HttpContext.Session.SetString("Bookings", JsonConvert.SerializeObject(_bookings));
        }
    }
}