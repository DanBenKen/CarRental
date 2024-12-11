using CarRental.Models;
using CarRental.Models.ViewModels.Car;
using CarRental.Services.Interfaces;

namespace CarRental.Services
{
    public class CarService : ICarService
    {
        private readonly CarRentalContext _context;

        public CarService(CarRentalContext context)
        {
            _context = context;
        }

        public PaginatedIndexViewModel GetFilteredCars(string make, string model, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
        {
            var carsQuery = _context.Cars.AsQueryable();

            bool hasMake = !string.IsNullOrWhiteSpace(make);
            bool hasModel = !string.IsNullOrWhiteSpace(model);
            bool hasMinPrice = minPrice.HasValue;
            bool hasMaxPrice = maxPrice.HasValue;

            if (hasMake || hasModel || hasMinPrice || hasMaxPrice)
            {
                if (hasMake)
                {
                    carsQuery = carsQuery.Where(c => c.Make.ToLower().Contains(make.Trim().ToLower()));
                }

                if (hasModel)
                {
                    carsQuery = carsQuery.Where(c => c.Model.ToLower().Contains(model.Trim().ToLower()));
                }

                if (hasMinPrice)
                {
                    carsQuery = carsQuery.Where(c => c.PricePerDay >= minPrice.Value);
                }

                if (hasMaxPrice)
                {
                    carsQuery = carsQuery.Where(c => c.PricePerDay <= maxPrice.Value);
                }
            }

            int totalItems = carsQuery.Count();

            var cars = carsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedIndexViewModel
            {
                Cars = cars,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                TotalItems = totalItems
            };
        }

        public IEnumerable<Car> GetAllCars()
        {
            return _context.Cars.ToList();
        }

        public Car GetCarById(int id)
        {
            return _context.Cars.Find(id);
        }

        public void AddCar(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public void UpdateCar(Car car)
        {
            _context.Cars.Update(car);
            _context.SaveChanges();
        }

        public void DeleteCar(int id)
        {
            var car = _context.Cars.Find(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
        }
    }
}
