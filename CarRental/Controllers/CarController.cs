using CarRental.Models.ViewModels.Car;
using CarRental.Services.Interfaces;
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
            var viewModel = _carService.GetFilteredCars(make, model, minPrice, maxPrice, page, pageSize);
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
