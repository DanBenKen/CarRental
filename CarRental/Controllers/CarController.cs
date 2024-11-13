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
                PricePerDay = 225.5m,
                IsAvailable = true,
                Seats = 5,
                ImageUrl = "lancerEvoVI.jpg"
            };

            Car imprezaSti = new Car()
            {
                CarId = 2,
                Make = "Subaru",
                Model = "Impreza WRX Sti",
                Year = 2006,
                PricePerDay = 224.5m,
                IsAvailable = false,
                Seats = 5,
                ImageUrl = "ImprezaWRXSti.jpg"
            };

            Car bmwM3 = new Car()
            {
                CarId = 3,
                Make = "BMW",
                Model = "M3 E46",
                Year = 2004,
                PricePerDay = 300.0m,
                IsAvailable = true,
                Seats = 4,
                ImageUrl = "M3E46.jpg"
            };

            Car audiRs6 = new Car()
            {
                CarId = 4,
                Make = "Audi",
                Model = "RS6 Avant",
                Year = 2020,
                PricePerDay = 500.0m,
                IsAvailable = true,
                Seats = 5,
                ImageUrl = "RS6Avant.jpg"
            };

            Car toyotaSupra = new Car()
            {
                CarId = 5,
                Make = "Toyota",
                Model = "Supra MK4",
                Year = 1998,
                PricePerDay = 275.0m,
                IsAvailable = false,
                Seats = 2,
                ImageUrl = "SupraMK4.jpg"
            };

            Car fordMustang = new Car()
            {
                CarId = 6,
                Make = "Ford",
                Model = "Mustang GT",
                Year = 2018,
                PricePerDay = 350.0m,
                IsAvailable = true,
                Seats = 4,
                ImageUrl = "MustangGT.jpg"

            };

            cars.Add(lancerEvo6);
            cars.Add(imprezaSti);
            cars.Add(bmwM3);
            cars.Add(audiRs6);
            cars.Add(toyotaSupra);
            cars.Add(fordMustang);

            return cars;
        }

    }
}
