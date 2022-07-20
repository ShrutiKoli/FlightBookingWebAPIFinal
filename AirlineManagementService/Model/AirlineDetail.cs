using System;
using System.Collections.Generic;

namespace AirlineManagementService.Model
{
    public partial class AirlineDetail
    {
        public int Id { get; set; }
        public string AirlineName { get; set; } = null!;
        public string? AirlineCode { get; set; }
        public byte[]? Logo { get; set; }
        public string? Address { get; set; }
        public string? About { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? LastupdatedBy { get; set; }
        public DateTime? LastupdatedOn { get; set; }
    }
}
