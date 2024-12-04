using CarRental.Models.ViewModels.Booking;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class BookingController : Controller
    {
        private readonly CarRentalContext _context;

        public BookingController(CarRentalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create(int carId)
        {
            int userId = 1;

            var car = _context.Cars.FirstOrDefault(c => c.CarId == carId);
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

            var car = _context.Cars.FirstOrDefault(c => c.CarId == bookingModel.CarId);
            if (car == null) return NotFound("Car not found.");

            var totalPrice = CalculateTotalPrice(bookingModel.StartDate, bookingModel.EndDate, bookingModel.PricePerDay);

            var newBooking = new Booking
            {
                CarId = bookingModel.CarId,
                UserId = userId,
                StartDate = bookingModel.StartDate,
                EndDate = bookingModel.EndDate,
                TotalPrice = totalPrice,
            };

            _context.Bookings.Add(newBooking);
            _context.SaveChanges();

            car.Status = "Rented";

            return RedirectToAction("Confirmation", new { bookingId = newBooking.BookingId });
        }

        [HttpGet]
        public IActionResult History(int page = 1, int pageSize = 3)
        {
            var userId = 1;

            var userBookingsQuery = _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Car)
                .Select(b => new HistoryViewModel
                {
                    BookingId = b.BookingId,
                    Make = b.Car.Make,
                    Model = b.Car.Model,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    TotalPrice = b.TotalPrice
                });


            int totalItems = userBookingsQuery.Count();

            var bookings = userBookingsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new PaginatedHistoryViewModel
            {
                Bookings = bookings,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                TotalItems = totalItems
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Confirmation(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound("Booking not found.");

            var car = _context.Cars.FirstOrDefault(c => c.CarId == booking.CarId);
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
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound("Booking not found.");

            var car = _context.Cars.FirstOrDefault(c => c.CarId == booking.CarId);
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

            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingModel.BookingId);
            if (booking == null) return NotFound("Booking not found.");

            var car = _context.Cars.FirstOrDefault(c => c.CarId == booking.CarId);
            if (car == null) return NotFound("Car not found.");

            _context.SaveChanges();

            car.Status = "Available";

            return RedirectToAction("History");
        }

        public decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, decimal pricePerDay)
        {
            var totalDays = (endDate - startDate).Days;

            if (totalDays < 1) totalDays = 1;

            var totalPrice = totalDays * pricePerDay;

            return totalPrice;
        }
    }
}