using CarRental.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        public IActionResult Index(string make, string model, decimal? minPrice, decimal? maxPrice)
        {
            var cars = InitCars();

            bool hasMake = !string.IsNullOrWhiteSpace(make);
            bool hasModel = !string.IsNullOrWhiteSpace(model);
            bool hasMinPrice = minPrice.HasValue;
            bool hasMaxPrice = maxPrice.HasValue;

            if (hasMake || hasModel || hasMinPrice || hasMaxPrice)
            {
                if (hasMake)
                {
                    cars = cars.Where(c => c.Make.ToLower().Contains(make.Trim().ToLower())).ToList();
                }

                if (hasModel)
                {
                    cars = cars.Where(c => c.Model.ToLower().Contains(model.Trim().ToLower())).ToList();
                }

                if (hasMinPrice)
                {
                    cars = cars.Where(c => c.PricePerDay >= minPrice.Value).ToList();
                }

                if (hasMaxPrice)
                {
                    cars = cars.Where(c => c.PricePerDay <= maxPrice.Value).ToList();
                }
            }

            return View(cars);
        }

        public IActionResult Details(int id)
        {
            var cars = InitCars();

            var car = cars.FirstOrDefault(c => c.CarId == id);
            if (car == null) return NotFound();

            return View(car);
        }

        private IEnumerable<Car> InitCars()
        {
            var cars = new List<Car>();

            Car lancerEvo6 = new Car()
            {
                CarId = 1,
                Make = "Mitsubishi",
                Model = "Lancer Evo VI",
                Year = 1999,
                PricePerDay = 224.5m,
                IsAvailable = true,
                Seats = 5
            };

            Car imprezaSti = new Car()
            {
                CarId = 2,
                Make = "Subaru",
                Model = "Impreza WRX Sti",
                Year = 2006,
                PricePerDay = 224.5m,
                IsAvailable = false,
                Seats = 5
            };

            cars.Add(lancerEvo6);
            cars.Add(@imprezaSti);

            return cars;
        }
    }
}
