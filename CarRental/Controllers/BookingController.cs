using CarRental.Models.ViewModels.Booking;
using Microsoft.AspNetCore.Mvc;
using CarRental.Services.Interfaces;

namespace CarRental.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int carId)
        {
            int userId = 1;
            var createViewModel = await _bookingService.GetCreateAsync(carId, userId);
            return View(createViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel bookingModel)
        {
            if (!ModelState.IsValid)
                return View(bookingModel);

            int userId = 1;
            int bookingId = await _bookingService.CreateBookingAsync(bookingModel, userId);
            return RedirectToAction("Confirmation", new { bookingId });
        }

        [HttpGet]
        public async Task<IActionResult> History(int page = 1, int pageSize = 6)
        {
            int userId = 1;
            var viewModel = await _bookingService.GetUserBookingHistoryAsync(userId, page, pageSize);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(int bookingId)
        {
            var viewModel = await _bookingService.GetConfirmationAsync(bookingId);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Return(int bookingId)
        {
            var returnViewModel = await _bookingService.GetReturnAsync(bookingId);
            return View(returnViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Return(ReturnViewModel bookingModel)
        {
            if (!ModelState.IsValid) return View(bookingModel);

            await _bookingService.SetReturn(bookingModel);
            return RedirectToAction("History");
        }
    }
}