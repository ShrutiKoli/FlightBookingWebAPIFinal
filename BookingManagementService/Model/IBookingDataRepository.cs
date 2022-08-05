using Microsoft.AspNetCore.Mvc;

namespace BookingManagementService.Model
{
    public interface IBookingDataRepository
    {
        public Task<ActionResult<BookingDetail>>BookFlight(BookingDetail bookingDetail);
        public Task<ActionResult<UserBookingDetail>> AddUserData(List<UserBookingDetail> users);
        public List<BookFlight> GetHistory(string Email);
        public BookFlight GetFlightDetails(string Pnr);
        public Task<ActionResult<string>> CancelBooking(int id);
        public Task<ActionResult<string>> ApplyDiscount(string discountCode);
    }
}
