using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace AirlineManagementService.Model
{
    public class AirlineDataRepository : IAirlineDataRepository
    {
        private readonly AirlineDBContext _context;

        public AirlineDataRepository(AirlineDBContext context)
        {
            _context = context;
        }
        public List<AirlineDetail> GetAirlines()
        {
            List<AirlineDetail> list= _context.AirlineDetails.ToList();

            return list;
        }
        public List<FlightScheduleData> GetAirlineSchedule()
        {
            List<FlightScheduleData> list = (from fs in _context.FlightScheduleDetails
                                             join airline in _context.AirlineDetails on fs.AirlineId equals airline.Id
                                             join flight in _context.FlightDetails on fs.FlightId equals flight.Id
                                             select new FlightScheduleData()
                                             { 
                                                 AirlineId=airline.Id,
                                                 AirlineName=airline.AirlineName,
                                                 FlightCode = flight.FlightCode,
                                                 FlightSchId=fs.Id,
                                                 Source=fs.Source,
                                                 Destination=fs.Destination,
                                                 FromDate=fs.FromDate,
                                                 ToDate=fs.ToDate,
                                                 ScheduledDays=fs.ScheduledDays,
                                                 Price=fs.Price,
                                                 status=fs.IsActive,
                                                 FlightId=flight.Id
                                             }).ToList();
            return list;
        }
        public List<FlightData> GetFlights()
        {
            List<FlightData> list = (
                                     from airline in _context.AirlineDetails 
                                     join flight in _context.FlightDetails on airline.Id equals flight.AirlineId
                                     select new FlightData()
                                     {   AirlineId=flight.AirlineId,
                                         AirlineName=airline.AirlineName,
                                         FlightCode=flight.FlightCode,
                                         FlightId=flight.Id,
                                         status=flight.IsActive
                                     }).ToList();

            return list;
        }

        public int GetAirlineByName(string name)
        {
            return _context.AirlineDetails.FirstOrDefault(a=>a.AirlineName==name).Id;
        }
        public async Task<ActionResult<AirlineDetail>> AddAirline(AirlineDetail airlineDetail)
        {
            _context.AirlineDetails.Add(airlineDetail);
            await _context.SaveChangesAsync();

            return airlineDetail;

        }

        public async Task<ActionResult<FlightDetail>> AddFlight(FlightDetail flightDetail)
        {
            _context.FlightDetails.Add(flightDetail);
            await _context.SaveChangesAsync();

            return flightDetail;
        }

        public async Task<ActionResult<string>> BlockAirline(string airlineName, string airlineCode,bool status)
        {
            AirlineDetail airline =  _context.AirlineDetails.FirstOrDefault(f => f.AirlineName == airlineName && f.AirlineCode == airlineCode);

            if (airline == null) return "NotFound";
            airline.IsActive = status;
            _context.FlightDetails.Where(f => f.AirlineId == airline.Id).ToList().ForEach(s=>s.IsActive=status);
            
            _context.Entry(airline).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return "OK";
        }

        public async Task<ActionResult<string>> BlockFlight(string flightcode, bool status)
        {
            FlightDetail flight = _context.FlightDetails.FirstOrDefault(f =>f.FlightCode==flightcode);

            if (flight == null) return "NotFound";
            flight.IsActive = status;
            _context.Entry(flight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return "OK";
        }

       
        public async Task<ActionResult<string>> ProvideDiscount()
        {
            IConnection con = AirlineQueue.GetConnection();
           

            string discountCode = await AirlineQueue.receive(con, "discount-queue");

            if (discountCode == null)
                return "BadRequest";
           

            var discount =_context.DiscountMasters.Where(d => d.DiscountCode == discountCode).FirstOrDefault().DiscountPercentage;
            if (discount != null)
            {
                bool res = await AirlineQueue.send(con, discount.ToString(), "discount-return-queue");
                if (res)
                {
                    return "Ok";

                }
            }
            return "Not Valid";
        }

      

        public async Task<ActionResult<FlightScheduleDetail>> ScheduleFlight(FlightScheduleDetail flightScheduleDetail)
        {
            _context.FlightScheduleDetails.Add(flightScheduleDetail);
            await _context.SaveChangesAsync();
       
            return flightScheduleDetail;
        }

        public List<FlightSearch> SearchFlight(string source, string destination, string date)
        {
            DateTime datetime = Convert.ToDateTime(date);
            List<FlightSearch> list = (from fs in _context.FlightScheduleDetails
                                       join airline in _context.AirlineDetails on fs.AirlineId equals airline.Id
                                       join flight in _context.FlightDetails on fs.FlightId equals flight.Id
                                       where fs.Source == source && fs.Destination == destination && datetime >= fs.FromDate && datetime <= fs.ToDate && airline.IsActive==true && flight.IsActive==true 
                                       select new FlightSearch()
                                       {
                                           AirlineName = airline.AirlineName,
                                           AirlineLogo = airline.Logo,
                                           FlightCode = flight.FlightCode,
                                           From = fs.FromDate,
                                           To = fs.ToDate,
                                           Source=fs.Source,
                                           Destination=fs.Destination,
                                           Price=fs.Price
                                          
                                       }).ToList();

            return list;
        }
    }
}
