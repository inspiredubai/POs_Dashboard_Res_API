using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POs_Dashboard_Res_API.Models
{
    public class VoidTransaction
    {
        public long? OrderID { get; set; }
    public DateTime? DateAndTime { get; set; }
    public string? ItemName { get; set; }
    public double? Amount { get; set; }
    public string? UserName { get; set; }
    public long? OutLetID { get; set; }
    public decimal? Quantity { get; set; }
    }
}