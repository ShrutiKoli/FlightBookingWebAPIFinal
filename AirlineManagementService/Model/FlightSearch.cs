namespace AirlineManagementService.Model
{
    public class FlightSearch
    {
        public string AirlineName { get; set; }
        public byte[] AirlineLogo { get; set; }

        public string FlightCode { get; set; }
        public DateTime ? From { get; set; } 
        public DateTime? To { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public decimal? Price { get; set; }



    }

    public class FlightData
    {
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public string FlightCode { get; set; }
        public bool? status { get; set; }
       public int FlightId { get; set; }
    }
    public class FlightScheduleData
    {
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public string FlightCode { get; set; }
        public int FlightId { get; set; }
        public int FlightSchId { get; set; }
        public bool? status { get; set; }

        public string? Source { get; set; }
        public string? Destination { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? ScheduledDays { get; set; }
        public decimal? Price { get; set; }


    }
}
