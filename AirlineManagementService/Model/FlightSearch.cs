namespace AirlineManagementService.Model
{
    public class FlightSearch
    {
        public string AirlineName { get; set; }
        public byte[] AirlineLogo { get; set; }

        public string FlightCode { get; set; }
        public DateTime ? From { get; set; } 
        public DateTime? To { get; set; }



    }
}
