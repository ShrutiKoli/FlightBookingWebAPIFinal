using BookingManagementService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingManagementService.Controllers
{
   [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingManagementController : ControllerBase
    {
        private readonly BookingDataRepository repository;

        public BookingManagementController(BookingDetailsDBContext context)
        {
            repository = new BookingDataRepository(context);
        }
        
        [HttpGet]
        [Route("GetHistory")]
        public List<BookFlight> GetHistory(string Email)
        {
           return repository.GetHistory(Email);
        }

        [HttpGet]
        [Route("GetFlightDetails")]
        public List<BookFlight> GetFlightDetails(string Pnr)
        {
             List<BookFlight> flights = new List<BookFlight>();
            var data = repository.GetFlightDetails(Pnr);
            if (data != null)
            {
                flights.Add(data);
                return flights;
            }
           else
                return flights;
        }

        

        [HttpPut]
        [Route("Cancel")]
        public async Task<ActionResult>CancelBooking(int boookingid )
        {
         var res =await repository.CancelBooking(boookingid);

            return Ok(res);
        }

        [HttpPost]
        [Route("bookflight")]
        public async Task<ActionResult<BookingDetail>> PostBookingDetail([FromBody] BookingDetail bookingDetail)
        {
           await repository.BookFlight(bookingDetail);
            List<UserBookingDetail> users = bookingDetail.UserBookingDetails.ToList();
          
            return bookingDetail;
        }
        [HttpPost]
        [Route("adduser")]
        public async Task<ActionResult<UserBookingDetail>> AddUserDetails([FromBody] List<UserBookingDetail> users)
        {

            await repository.AddUserData(users);
             return Ok();
        }


        [HttpPost]
        [Route("ApplyDiscount")]
        public async Task<ActionResult<string>> ApplyDiscount(string discountCode)
        {
           return await repository.ApplyDiscount(discountCode);

        }

        [HttpGet]
        [Route("GetDiscount")]
        public async Task<ActionResult<string>> GetDiscount()
        {
            return await repository.GetDiscount();

        }

        #region
        /*
                // GET: api/<BookingManagementController>
                [HttpGet]
                public IEnumerable<string> Get()
                {
                    return new string[] { "value1", "value2" };
                }

                // GET api/<BookingManagementController>/5
                [HttpGet("{id}")]
                public string Get(int id)
                {
                    return "value";
                }

                // PUT api/<BookingManagementController>/5
                [HttpPut("{id}")]
                public void Put(int id, [FromBody] string value)
                {
                }

                // DELETE api/<BookingManagementController>/5
                [HttpDelete("{id}")]
                public void Delete(int id)
                {
                }
                // POST api/<BookingManagementController>
                [HttpPost]
                public void Post([FromBody] string value)
                {
                }
        */
        #endregion
    }
}
