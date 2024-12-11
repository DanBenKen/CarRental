using CarRental.Models;
using CarRental.Models.ViewModels.Car;

namespace CarRental.Services.Interfaces
{
    public interface ICarService
    {
        IEnumerable<Car> GetAllCars();
        Car GetCarById(int id);
        void AddCar(Car car);
        void UpdateCar(Car car);
        void DeleteCar(int id);
        PaginatedIndexViewModel GetFilteredCars(string make, string model, decimal? minPrice, decimal? maxPrice, int page, int pageSize);
    }
}
