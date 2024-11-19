using CarRental.Models;
using CarRental.Models.ViewModels.Booking;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class BookingController : Controller
    {
        private List<Car> _cars;
        private List<Booking> _bookings;

        public BookingController()
        {
            _cars = InitCars().ToList();
            _bookings = InitBooking().ToList();
        }

        [HttpGet]
        public IActionResult Create(int carId)
        {
            var userId = 1;

            var car = _cars.FirstOrDefault(c => c.CarId == carId);
            if (car == null || !car.IsAvailable)
            {
                return NotFound();
            }

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
            if (ModelState.IsValid)
            {
                decimal totalPrice = CalculateTotalPrice(bookingModel.StartDate, bookingModel.EndDate, bookingModel.PricePerDay);

                var newBooking = new Booking
                {
                    BookingId = _bookings.Max(b => b.BookingId) + 1,
                    CarId = bookingModel.CarId,
                    UserId = bookingModel.UserId,
                    StartDate = bookingModel.StartDate,
                    EndDate = bookingModel.EndDate,
                    TotalPrice = totalPrice,
                };

                _bookings.Add(newBooking);

                var car = _cars.FirstOrDefault(c => c.CarId == bookingModel.CarId);
                if (car == null) return NotFound();

                car.IsAvailable = false;

                return RedirectToAction("History");
            }

            return View(bookingModel);
        }

        [HttpGet]
        public IActionResult History()
        {
            var userId = 1;

            var userBookings = _bookings.Where(b => b.UserId == userId).Select(b =>
            {           
                var car = _cars.FirstOrDefault(c => c.CarId == b.CarId);

                return new HistoryViewModel
                {
                    BookingId = b.BookingId,
                    Make = car.Make,
                    Model = car.Model,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    TotalPrice = b.TotalPrice,
                    IsAvailable = car.IsAvailable
                };
            }).ToList();

            return View(userBookings);
        }

        [HttpGet]
        public IActionResult Return(int bookingId)
        {
            var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null) return NotFound();

            var car = _cars.FirstOrDefault(c => c.CarId == booking.CarId);
            if (car == null) return NotFound();

            var returnViewModel = new ReturnViewModel
            {
                BookingId = booking.BookingId,
                Make = car.Make,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                IsAvailable = car.IsAvailable,
            };

            return View(returnViewModel);
        }

        [HttpPost]
        public IActionResult Return(ReturnViewModel bookingModel)
        {
            if (ModelState.IsValid)
            {
                var booking = _bookings.FirstOrDefault(b => b.BookingId == bookingModel.BookingId);
                if (booking == null) return NotFound();

                var car = _cars.FirstOrDefault(c => c.CarId == booking.CarId);
                if (car == null) return NotFound();

                car.IsAvailable = true;

                return RedirectToAction("History");
            }

            return View(bookingModel);
        }

        private decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, decimal pricePerDay)
        {
            if (endDate < startDate || pricePerDay <= 0)
            {
                return 0;
            }

            int rentalDays = (endDate - startDate).Days + 1;

            return rentalDays * pricePerDay;
        }


        private decimal GetCarPriceById(int cardId)
        {
            var car = _cars.FirstOrDefault(c => c.CarId == cardId);
            return car != null ? car.PricePerDay : 0;
        }

        private IEnumerable<Booking> InitBooking()
        {
            return new List<Booking>
            {
                new Booking { BookingId = 1, CarId = 2, UserId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(7), TotalPrice = CalculateTotalPrice(DateTime.Now, DateTime.Now.AddDays(7), GetCarPriceById(2)) },
                new Booking { BookingId = 2, CarId = 5, UserId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10), TotalPrice = CalculateTotalPrice(DateTime.Now, DateTime.Now.AddDays(10), GetCarPriceById(3)) },
                new Booking { BookingId = 3, CarId = 4, UserId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), TotalPrice = CalculateTotalPrice(DateTime.Now, DateTime.Now.AddDays(5), GetCarPriceById(4)) },
            };
        }

        private IEnumerable<Car> InitCars()
        {
            return new List<Car>
            {
                new Car { CarId = 1, Make = "Mitsubishi", Model = "Lancer Evo VI", Year = 1999, PricePerDay = 225.5m, IsAvailable = true, Seats = 5, FuelType = "Petrol", ImageUrl = "lancerEvoVI.jpg" },
                new Car { CarId = 2, Make = "Subaru", Model = "Impreza WRX Sti", Year = 2006, PricePerDay = 224.5m, IsAvailable = false, Seats = 5, FuelType = "Petrol", ImageUrl = "SubaruImprezaWRXSti.jpg" },
                new Car { CarId = 3, Make = "BMW", Model = "M3 E46", Year = 2004, PricePerDay = 300.0m, IsAvailable = true, Seats = 4, FuelType = "Petrol", ImageUrl = "BMWM3E46.jpg" },
                new Car { CarId = 4, Make = "Audi", Model = "RS6 Avant", Year = 2020, PricePerDay = 500.0m, IsAvailable = true, Seats = 5, FuelType = "Petrol", ImageUrl = "AudiRS6Avant.jpg" },
                new Car { CarId = 5, Make = "Toyota", Model = "Supra MK4", Year = 1998, PricePerDay = 275.0m, IsAvailable = false, Seats = 2, FuelType = "Petrol", ImageUrl = "ToyotaSupraMK4.jpg" },
                new Car { CarId = 6, Make = "Ford", Model = "Mustang GT", Year = 2018, PricePerDay = 350.0m, IsAvailable = false, Seats = 4, FuelType = "Petrol", ImageUrl = "FordMustangGT.jpg" }
            };
        }
    }
}
