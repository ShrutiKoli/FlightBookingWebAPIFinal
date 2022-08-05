using AirlineManagementService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AirlineManagementService.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AirlineManagementController : ControllerBase
    {

        private readonly AirlineDataRepository repository;

        public AirlineManagementController(AirlineDBContext context)
        {
            repository = new AirlineDataRepository(context);
        }

  
        // GET: api/<AirlineManagementController>

        [HttpGet]
        [AllowAnonymous]
        [Route("SearchFlight")]
        public List<FlightSearch> SearchFlight(string source, string destination, string date)
        {
            return repository.SearchFlight(source, destination, date);
        }
        [HttpGet]
      
        [Route("getAirlines")]
        public List<AirlineDetail> GetAirlines()
        {
            return repository.GetAirlines();
        }
        [HttpGet]

        [Route("getFlights")]
        public List<FlightData> GetFlights()
        {
            return repository.GetFlights();
        }

        [HttpGet]
        [Route("getAirlineSchedule")]
        public List<FlightScheduleData> GetAirlineSchedule()
        {
            return repository.GetAirlineSchedule();
        }
        [HttpGet]

        [Route("getAirlineByName")]
        public int GetAirlineByName(string name)
        {
            return repository.GetAirlineByName(name);
        }



        // POST api/<AirlineManagementController>
        [HttpPost]
        [Route("AddAirline")]
        public async Task<IActionResult>AddAirline([FromBody] AirlineDetail airlineDetail)
        {
            AirlineDetail airline = new AirlineDetail()
            {
                AirlineName= airlineDetail.AirlineName,
                AirlineCode= airlineDetail.AirlineCode,
                Logo=airlineDetail.Logo,
                Address=airlineDetail.Address,
                About= airlineDetail.About,
                IsActive=true,
                CreatedBy=null,
                CreatedOn=DateTime.Now,
                LastupdatedBy=null,
                LastupdatedOn=DateTime.Now
            };
            await repository.AddAirline(airline);
            return Ok();
        }

        [HttpPost]
        [Route("AddFlight")]
        public async Task<IActionResult> AddFlight([FromBody] FlightDetail flightDetail)
        {
            FlightDetail flight = new FlightDetail()
            {
                AirlineId= flightDetail.AirlineId, 
                FlightCode= flightDetail.FlightCode,
                IsActive=true,
                CreatedBy=flightDetail.CreatedBy,
                CreatedOn=DateTime.Now,
                LastupdatedBy=flightDetail.LastupdatedBy,
                LastupdatedOn=DateTime.Now

            };
            await repository.AddFlight(flight);
            return Ok();
        }

        [HttpPost]
        [Route("ScheduleFlight")]
        public async Task<IActionResult> ScheduleFlight([FromBody] FlightScheduleDetail flightScheduleDetail)
        {
            
            FlightScheduleDetail flightSchedule = new FlightScheduleDetail()
            {
                FlightId= flightScheduleDetail.FlightId,
                AirlineId= flightScheduleDetail.AirlineId,
                Source=flightScheduleDetail.Source,
                Destination=flightScheduleDetail.Destination,
                FromDate=flightScheduleDetail.FromDate,
                ToDate=flightScheduleDetail.ToDate,
                ScheduledDays=flightScheduleDetail.ScheduledDays,
                InstrumentId=flightScheduleDetail.InstrumentId,
                NoOfBussinessSeats=flightScheduleDetail.NoOfBussinessSeats,
                NoOfNonBussinessSeats=flightScheduleDetail.NoOfNonBussinessSeats,
                Price=flightScheduleDetail.Price,
                IsActive = true,
                CreatedBy = flightScheduleDetail.CreatedBy,
                CreatedOn = DateTime.Now,
                LastupdatedBy = flightScheduleDetail.LastupdatedBy,
                LastUpdatedOn = DateTime.Now

            };
            await repository.ScheduleFlight(flightSchedule);
            return Ok();
        }

        [HttpPut]
        [Route("BlockAirline")]
        public async Task<IActionResult> BlockAirline(string airlineName, string airlineCode, bool status)
        {
            var res=await repository.BlockAirline(airlineName, airlineCode,status);

            return Ok(res);


        }

        [HttpPut]
        [Route("BlockFlight")]
        public async Task<IActionResult> BlockFlight( string flightcode, bool status)
        {
            var res = await repository.BlockFlight(flightcode,status);

            return Ok(res);


        }

        
        [HttpPost]
        [Route("ProvideDiscount")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> ProvideDiscount()
        {
            return await repository.ProvideDiscount();

        }
        #region 1
        //// GET api/<AirlineManagementController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        //// PUT api/<AirlineManagementController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AirlineManagementController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        #endregion 1
    }
}
