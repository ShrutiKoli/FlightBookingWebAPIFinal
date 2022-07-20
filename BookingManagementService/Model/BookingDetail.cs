using System;
using System.Collections.Generic;

namespace BookingManagementService.Model
{
    public partial class BookingDetail
    {
        public BookingDetail()
        {
            UserBookingDetails = new HashSet<UserBookingDetail>();
        }

        public int Id { get; set; }
        public string? Pnr { get; set; }
        public int? FlightId { get; set; }
        public int? AirlineId { get; set; }
        public int? FlightSchId { get; set; }
        public string? NameOfUser { get; set; }
        public string? EmailId { get; set; }
        public int? NoOfSeats { get; set; }
        public double? Discount { get; set; }
        public decimal? Cost { get; set; }
        public string? MealOption { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public virtual ICollection<UserBookingDetail> UserBookingDetails { get; set; }
    }
}
