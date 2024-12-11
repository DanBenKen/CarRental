using CarRental.Models;
using CarRental.Models.ViewModels.Booking;
using CarRental.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Services
{
    public class BookingService : IBookingService
    {
        public readonly CarRentalContext _context;
        public BookingService(CarRentalContext context) 
        {
            _context = context;
        }

        public async Task<CreateViewModel> GetCreateAsync(int carId, int userId)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == carId);
            if (car == null) throw new Exception("Car not found.");

            return new CreateViewModel
            {
                UserId = userId,
                CarId = car.CarId,
                Make = car.Make,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
            };
        }

        public async Task<int> CreateBookingAsync(CreateViewModel bookingModel, int userId)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == bookingModel.CarId);
            if (car == null) throw new Exception("Car not found.");

            var totalPrice = CalculateTotalPrice(bookingModel.StartDate, bookingModel.EndDate, bookingModel.PricePerDay);

            var newBooking = new Booking
            {
                CarId = bookingModel.CarId,
                UserId = userId,
                StartDate = bookingModel.StartDate,
                EndDate = bookingModel.EndDate,
                TotalPrice = totalPrice,
                Status = "Rented",
            };

            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            return newBooking.BookingId;
        }

        public async Task<PaginatedHistoryViewModel> GetUserBookingHistoryAsync(int userId, int page = 1, int pageSize = 6)
        {
            var userBookingsQuery = _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Car)
                .Select(b => new HistoryViewModel
                {
                    BookingId = b.BookingId,
                    Make = b.Car.Make,
                    Model = b.Car.Model,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status,
                });

            int totalItems = await userBookingsQuery.CountAsync();

            var bookings = await userBookingsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedHistoryViewModel
            {
                Bookings = bookings,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                TotalItems = totalItems
            };
        }

        public async Task<ConfirmationViewModel> GetConfirmationAsync(int bookingId)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
            if (booking == null) throw new Exception("Booking not found.");

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == booking.CarId);
            if (car == null) throw new Exception("Car not found.");

            return new ConfirmationViewModel
            {
                BookingId = booking.BookingId,
                Make = car.Make,
                Model = car.Model,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                TotalPrice = booking.TotalPrice,
            };
        }

        public async Task<ReturnViewModel> GetReturnAsync(int bookingId)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
            if (booking == null) throw new Exception("Booking not found.");

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == booking.CarId);
            if (car == null) throw new Exception("Car not found.");

            return new ReturnViewModel
            {
                BookingId = booking.BookingId,
                Make = car.Make,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
            };
        }

        public async Task SetReturn(ReturnViewModel bookingModel)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingModel.BookingId);
            if (booking == null) throw new Exception("Booking not found.");

            var car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == booking.CarId);
            if (car == null) throw new Exception("Car not found.");

            booking.Status = "Returned";

            await _context.SaveChangesAsync();
        }

        public decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, decimal pricePerDay)
        {
            var totalDays = (endDate - startDate).Days;

            if (totalDays < 1) totalDays = 1;

            var totalPrice = totalDays * pricePerDay;

            return totalPrice;
        }
    }
}
