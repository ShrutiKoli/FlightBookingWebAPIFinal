using System;
using System.Collections.Generic;

namespace BookingManagementService.Model
{
    public partial class UserBookingDetail
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? SeatNumber { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public virtual BookingDetail Booking { get; set; } = null!;
    }
}
