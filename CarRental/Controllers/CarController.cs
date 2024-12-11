using CarRental.Models.ViewModels.Car;
using CarRental.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        public IActionResult Index(string make, string model, decimal? minPrice, decimal? maxPrice, int page = 1, int pageSize = 6)
        {
            var carsQuery = _carService.GetAllCars();

            bool hasMake = !string.IsNullOrWhiteSpace(make);
            bool hasModel = !string.IsNullOrWhiteSpace(model);
            bool hasMinPrice = minPrice.HasValue;
            bool hasMaxPrice = maxPrice.HasValue;

            if (hasMake || hasModel || hasMinPrice || hasMaxPrice)
            {
                if (hasMake)
                {
                    carsQuery = carsQuery.Where(c => c.Make.ToLower().Contains(make.Trim().ToLower())).ToList();
                }

                if (hasModel)
                {
                    carsQuery = carsQuery.Where(c => c.Model.ToLower().Contains(model.Trim().ToLower())).ToList();
                }

                if (hasMinPrice)
                {
                    carsQuery = carsQuery.Where(c => c.PricePerDay >= minPrice.Value).ToList();
                }

                if (hasMaxPrice)
                {
                    carsQuery = carsQuery.Where(c => c.PricePerDay <= maxPrice.Value).ToList();
                }
            }

            int totalItems = carsQuery.Count();

            carsQuery = carsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new PaginatedIndexViewModel
            {
                Cars = carsQuery,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                TotalItems = totalItems
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var car = _carService.GetCarById(id);
            if (car == null) return NotFound();

            return View(car);
        }
    }
}
