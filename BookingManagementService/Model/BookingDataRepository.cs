using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text;

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
            bookingDetail.IsActive=true;
            bookingDetail.CreatedOn = DateTime.Now;
            bookingDetail.LastUpdatedOn = DateTime.Now;
            foreach(var u in bookingDetail.UserBookingDetails)
            {
                u.IsActive=true;
                u.LastUpdatedOn = DateTime.Now;
                u.CreatedOn = DateTime.Now;
            }
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
            foreach (UserBookingDetail user in users)
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
                                    
                                     where bookFlight.EmailId == Email
                                     
                                     select new BookFlight()
                                     {
                                         ID=bookFlight.Id,
                                         Pnr = bookFlight.Pnr,
                                         AirlineId = (int)bookFlight.AirlineId,
                                         Name = bookFlight.NameOfUser,
                                         AirlineName=bookFlight.AirlineName,
                                         FlightCode=bookFlight.FlightCode,
                                         AirlinePrice = (decimal)bookFlight.Cost,
                                         Source =bookFlight.Source,
                                         Destination = bookFlight.Destination,
                                         From = bookFlight.FromDate,
                                         To = bookFlight.ToDate,
                                         MealOption = bookFlight.MealOption,
                                         passengers = bookFlight.UserBookingDetails.ToList(),
                                         FlightSchID=bookFlight.FlightSchId,
                                         status=bookFlight.IsActive,
                                         BookedDate=bookFlight.CreatedOn

                                     }).ToList();
            return list;

        }

        public BookFlight GetFlightDetails(string Pnr)
        {
                var flight = (from bookFlight in _context.BookingDetails
                          join user in _context.UserBookingDetails on bookFlight.Id equals user.BookingId
                          where bookFlight.Pnr == Pnr

                          select new BookFlight()
                          {
                              ID = bookFlight.Id,
                              Pnr = bookFlight.Pnr,
                              AirlineId = bookFlight.AirlineId,
                              Name = bookFlight.NameOfUser,
                              AirlineName = bookFlight.AirlineName,
                              FlightCode = bookFlight.FlightCode,
                              AirlinePrice = bookFlight.Cost,
                              Source = bookFlight.Source,
                              Destination = bookFlight.Destination,
                              From = bookFlight.FromDate,
                              To = bookFlight.ToDate,
                              MealOption = bookFlight.MealOption,
                              passengers = bookFlight.UserBookingDetails.ToList(),
                              FlightSchID = bookFlight.FlightSchId,
                              status = bookFlight.IsActive,
                               BookedDate = bookFlight.CreatedOn

                          }).FirstOrDefault();
                return flight;

            
        }

        public async Task<ActionResult<string>> CancelBooking(int id)
        {
            BookingDetail bookingDetail = new BookingDetail();

            bookingDetail = _context.BookingDetails.FirstOrDefault(b => b.Id == id);
            if (bookingDetail == null)
            {
                return "Not Found";
            }
            double hours = (DateTime.Now - bookingDetail.CreatedOn).GetValueOrDefault().TotalHours;
            if (hours > 24)
            {
                return "You can cancel tickets within 24hrs only ";
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

        public async Task<ActionResult<string>> ApplyDiscount(string discountCode)
        {
            IConnection con=FlightBookQueue.GetConnection();

            bool res= await FlightBookQueue.send(con, discountCode, "discount-queue");
            if(res)
            return "Ok";

            return "BadRequest";

        }

        public async Task<ActionResult<string>> GetDiscount()
        {
            IConnection con = FlightBookQueue.GetConnection();
            
            string discount = await FlightBookQueue.receive(con, "discount-return-queue");
            if(discount!=null)
                return discount;

            
            return "No Data Found";

        }
    }
}
