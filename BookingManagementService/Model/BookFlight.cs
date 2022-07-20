namespace BookingManagementService.Model
{
    public class Passenger
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public string Gender { get; set; }
        public string SeatNumber { get; set; }
    }
    public class BookFlight
    {

        public int FlightSchID { get; set; }
        public int AirlineId { get; set; }
        public string Name { get; set; }
       public decimal AirlinePrice { get; set; }

        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string MealOption { get; set; }
        public List<UserBookingDetail> passengers { get; set; }

    }

  
}
