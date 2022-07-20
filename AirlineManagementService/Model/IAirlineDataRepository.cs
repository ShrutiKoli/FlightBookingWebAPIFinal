using Microsoft.AspNetCore.Mvc;

namespace AirlineManagementService.Model
{
    public interface IAirlineDataRepository
    {
        List<FlightSearch> SearchFlight(string source, string destination, string date);
        Task<ActionResult<AirlineDetail>> AddAirline(AirlineDetail airlineDetail);
        Task<ActionResult<FlightDetail>> AddFlight(FlightDetail flightDetail);
        Task<ActionResult<string>> BlockAirline(string irlineName,string airlineCode);
        Task<ActionResult<FlightScheduleDetail>> ScheduleFlight(FlightScheduleDetail flightScheduleDetail);
    }
}
