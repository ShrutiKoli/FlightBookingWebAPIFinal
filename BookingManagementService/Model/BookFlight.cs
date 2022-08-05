namespace BookingManagementService.Model
{
    public class Passenger
    {
        public int FlightSchID { get; set; } = 0;
        public int AirlineId { get; set; } = 0;
    }
    public class BookFlight
    {
        public int ID { get; set; }
        public int? FlightSchID { get; set; }
        public int? AirlineId { get; set; }
        public string? Name { get; set; }
        public string? Pnr { get; set; }
        public decimal? AirlinePrice { get; set; }
        public string? AirlineName { get; set; }
        public string? FlightCode { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public DateTime? BookedDate { get; set; }
        public string? MealOption { get; set; }
        public bool? status { get; set; }
        public List<UserBookingDetail> passengers { get; set; }

    }

  
}
