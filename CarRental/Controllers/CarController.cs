using CarRental.Models;
using CarRental.Models.ViewModels.Car;
using CarRental.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        private readonly Garage _garage;
        private readonly List<Car> _car;
        private readonly CarRentalContext _context;

        public CarController(Garage garage, CarRentalContext context)
        {
            _garage = garage;
            _car = _garage.GenerateCars().ToList();
            _context = context;
        }

        public IActionResult Index(string make, string model, decimal? minPrice, decimal? maxPrice, int page = 1, int pageSize = 6)
        {
            var carsQuery = _context.Cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(make))
            {
                carsQuery = carsQuery.Where(c => c.Make.ToLower().Contains(make.Trim().ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(model))
            {
                carsQuery = carsQuery.Where(c => c.Model.ToLower().Contains(model.Trim().ToLower()));
            }

            if (minPrice.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.PricePerDay >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                carsQuery = carsQuery.Where(c => c.PricePerDay <= maxPrice.Value);
            }

            int totalItems = carsQuery.Count();

            var cars = carsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new CarListViewModel
            {
                Cars = cars,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                TotalItems = totalItems
            };

            return View(viewModel);
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
