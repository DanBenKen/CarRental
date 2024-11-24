using CarRental.Models;

namespace CarRental.Utils
{
    public class Garage
    {
        private readonly List<Car> _cars;

        public Garage()
        {
            _cars = GenerateCars().ToList();
        }

        public IEnumerable<Booking> GenerateBookings()
        {
            return new List<Booking>
            {
                new Booking { BookingId = 1, CarId = 2, UserId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), TotalPrice = CalculateTotalPrice(DateTime.Now, DateTime.Now.AddDays(1), GetCarPriceById(2)) },
                new Booking { BookingId = 2, CarId = 4, UserId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10), TotalPrice = CalculateTotalPrice(DateTime.Now, DateTime.Now.AddDays(2), GetCarPriceById(3)) },
                new Booking { BookingId = 3, CarId = 6, UserId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(5), TotalPrice = CalculateTotalPrice(DateTime.Now, DateTime.Now.AddDays(3), GetCarPriceById(4)) },
            };
        }

        public IEnumerable<Car> GenerateCars()
        {
            return new List<Car>
            {
                new Car { CarId = 1, Make = "Mitsubishi", Model = "Lancer Evo VI", Year = 1999, PricePerDay = 225.5m, Seats = 5, FuelType = "Petrol", Status = "Available", ImageUrl = "lancerEvoVI.jpg" },
                new Car { CarId = 2, Make = "Subaru", Model = "Impreza WRX Sti", Year = 2006, PricePerDay = 224.5m, Seats = 5, FuelType = "Petrol", Status = "Rented", ImageUrl = "SubaruImprezaWRXSti.jpg" },
                new Car { CarId = 3, Make = "BMW", Model = "M3 E46", Year = 2004, PricePerDay = 300.0m, Seats = 4, FuelType = "Petrol", Status = "Available", ImageUrl = "BMWM3E46.jpg" },
                new Car { CarId = 4, Make = "Audi", Model = "RS6 Avant", Year = 2020, PricePerDay = 500.0m, Seats = 5, FuelType = "Petrol", Status = "Rented", ImageUrl = "AudiRS6Avant.jpg" },
                new Car { CarId = 5, Make = "Toyota", Model = "Supra MK4", Year = 1998, PricePerDay = 275.0m, Seats = 2, FuelType = "Petrol", Status = "Available", ImageUrl = "ToyotaSupraMK4.jpg" },
                new Car { CarId = 6, Make = "Ford", Model = "Mustang GT", Year = 2018, PricePerDay = 350.0m, Seats = 4, FuelType = "Petrol", Status = "Rented", ImageUrl = "FordMustangGT.jpg" },
                new Car { CarId = 7, Make = "Chevrolet", Model = "Corvette C7", Year = 2017, PricePerDay = 400.0m, Seats = 2, FuelType = "Petrol", Status = "Available", ImageUrl = "" }
            };
        }

        public decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, decimal pricePerDay)
        {
            int rentalDays = (endDate - startDate).Days;
            if (rentalDays == 0)
            {
                rentalDays = 1;
            }

            return rentalDays * pricePerDay;
        }

        private decimal GetCarPriceById(int cardId)
        {
            var car = _cars.FirstOrDefault(c => c.CarId == cardId);
            return car != null ? car.PricePerDay : 0;
        }
    }
}
