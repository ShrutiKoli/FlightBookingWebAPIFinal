using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingManagementService.Model
{
    public class BookingDataRepository : IBookingDataRepository

    {
        private readonly BookingDetailsDBContext _context;

        public BookingDataRepository(BookingDetailsDBContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<BookingDetail>> BookFlight(BookingDetail bookingDetail)
        {
            string pnr = null;
            _context.BookingDetails.Add(bookingDetail);
            try
            {
                await _context.SaveChangesAsync();
                pnr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + bookingDetail.Id;

                _context.BookingDetails.Where(b => b.Id == bookingDetail.Id).FirstOrDefault().Pnr = pnr;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return bookingDetail;
        
    }

        public async Task<ActionResult<UserBookingDetail>> AddUserData(List<UserBookingDetail> users)
        {
            foreach(UserBookingDetail user in users)
            {
                _context.UserBookingDetails.Add(user);
            }
          
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }

            return users.FirstOrDefault();

        }

        public List<BookFlight> GetHistory(string Email)
        {
            List<BookFlight> list = (from bookFlight in _context.BookingDetails
                                     join user in _context.UserBookingDetails on bookFlight.Id equals user.BookingId
                                     where bookFlight.EmailId == Email && bookFlight.IsActive == true && user.IsActive == true
                                   
                                     select new BookFlight()
                                     {
                                         AirlineId = (int)bookFlight.AirlineId,
                                         Name = bookFlight.NameOfUser,
                                         AirlinePrice = (decimal)bookFlight.Cost,
                                         Source = null,
                                         Destination = null,
                                         From = DateTime.Now,
                                         To = DateTime.Now,
                                         MealOption = bookFlight.MealOption,
                                        passengers = bookFlight.UserBookingDetails.ToList()

                                     }).ToList();
            return list;    

        }

        public BookFlight GetFlightDetails(string Pnr)
        {
            BookFlight flight = new BookFlight();
             flight = (from bookFlight in _context.BookingDetails
                                     join user in _context.UserBookingDetails on bookFlight.Id equals user.BookingId
                                     where bookFlight.Pnr == Pnr && bookFlight.IsActive == true && user.IsActive == true

                                     select new BookFlight()
                                     {
                                         AirlineId = (int)bookFlight.AirlineId,
                                         Name = bookFlight.NameOfUser,
                                         AirlinePrice = (decimal)bookFlight.Cost,
                                         Source = null,
                                         Destination = null,
                                         From = DateTime.Now,
                                         To = DateTime.Now,
                                         MealOption = bookFlight.MealOption,
                                         passengers = bookFlight.UserBookingDetails.ToList()

                                     }).FirstOrDefault();
            return flight;

        }

        public async Task<ActionResult<string>> CancelBooking(int id)
        {
            BookingDetail bookingDetail= new BookingDetail();
           
            bookingDetail =    _context.BookingDetails.FirstOrDefault(b => b.Id == id);
            double hours= (DateTime.Now- bookingDetail.CreatedOn).GetValueOrDefault().TotalHours;
            if(hours>24)
            {
                return "Can not cancel";
            }
            if (bookingDetail== null)
            {
                return "Not Found";
            }
            bookingDetail.IsActive = false;
            _context.Entry(bookingDetail).State = EntityState.Modified;
            _context.UserBookingDetails.Where(u => u.BookingId == id).ToList().ForEach(b => b.IsActive = false);
      

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return "Ok";
        }
    }
}
