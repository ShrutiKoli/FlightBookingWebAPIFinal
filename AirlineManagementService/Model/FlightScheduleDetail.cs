using System;
using System.Collections.Generic;

namespace AirlineManagementService.Model
{
    public partial class FlightScheduleDetail
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int AirlineId { get; set; }
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? ScheduledDays { get; set; }
        public long? InstrumentId { get; set; }
        public int? NoOfBussinessSeats { get; set; }
        public int? NoOfNonBussinessSeats { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? LastupdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}
