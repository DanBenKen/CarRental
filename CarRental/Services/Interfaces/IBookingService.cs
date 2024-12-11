using CarRental.Models.ViewModels.Booking;

namespace CarRental.Services.Interfaces
{
    public interface IBookingService
    {
        Task<int> CreateBookingAsync(CreateViewModel bookingModel, int userId);
        Task<CreateViewModel> GetCreateAsync(int carId, int userId);
        Task<PaginatedHistoryViewModel> GetUserBookingHistoryAsync(int userId, int page, int pageSize);
        Task<ConfirmationViewModel> GetConfirmationAsync(int bookingId);
        Task<ReturnViewModel> GetReturnAsync(int bookingId);
        Task SetReturn(ReturnViewModel bookingModel);
    }
}
