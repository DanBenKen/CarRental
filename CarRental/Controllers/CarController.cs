using CarRental.Models.ViewModels.Car;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        private readonly CarRentalContext _context;

        public CarController(CarRentalContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string make, string model, decimal? minPrice, decimal? maxPrice, int page = 1, int pageSize = 6)
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

            int totalItems = await carsQuery.CountAsync();

            var cars = await carsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new PaginatedIndexViewModel
            {
                Cars = cars,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                TotalItems = totalItems
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var car = await _context.Cars.SingleOrDefaultAsync(c => c.CarId == id);
            if (car == null) return NotFound();

            return View(car);
        }
    }
}
