using CarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options) { }

        public DbSet<Car> Cars {  get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews {  get; set; }
        public DbSet<User> Users { get; set; }
    }
}
