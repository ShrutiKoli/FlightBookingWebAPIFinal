using System;
using System.Collections.Generic;

namespace AirlineManagementService.Model
{
    public partial class DiscountMaster
    {
        public int Id { get; set; }
        public string? DiscountCode { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedtBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public double? DiscountPercentage { get; set; }
    }
}
