using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirlineManagementService.Model
{
    public class AirlineDataRepository : IAirlineDataRepository
    {
        private readonly AirlineDBContext _context;

        public AirlineDataRepository(AirlineDBContext context)
        {
            _context = context;
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

        public async Task<ActionResult<string>> BlockAirline(string airlineName, string airlineCode)
        {
            AirlineDetail airline =  _context.AirlineDetails.FirstOrDefault(f => f.AirlineName == airlineName && f.AirlineCode == airlineCode);

            if (airline == null) return "NotFound";
            airline.IsActive = false;
            _context.FlightDetails.Where(f => f.AirlineId == airline.Id).ToList().ForEach(s=>s.IsActive=false);
            
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
                                           To = fs.ToDate

                                       }).ToList();

            return list;
        }
    }
}
