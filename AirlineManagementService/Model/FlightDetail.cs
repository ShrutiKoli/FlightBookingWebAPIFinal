using System;
using System.Collections.Generic;

namespace AirlineManagementService.Model
{
    public partial class FlightDetail
    {
        public int Id { get; set; }
        public int AirlineId { get; set; }
        public string FlightCode { get; set; } = null!;
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? LastupdatedBy { get; set; }
        public DateTime? LastupdatedOn { get; set; }
    }
}
