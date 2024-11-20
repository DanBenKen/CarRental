using AspNetCoreGeneratedDocument;
using CarRental.Models;
using CarRental.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        private readonly Garage _garage;
        private readonly List<Car> _car;

        public CarController(Garage garage)
        {
            _garage = garage;
            _car = _garage.GenerateCars().ToList();
        }

        public IActionResult Index(string make, string model, decimal? minPrice, decimal? maxPrice)
        {
            var cars = _car;

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

        [HttpGet]
        public IActionResult Details(int id)
        {
            var cars = _car;

            var car = cars.SingleOrDefault(c => c.CarId == id);
            if (car == null) return NotFound();

            return View(car);
        }
    }
}
