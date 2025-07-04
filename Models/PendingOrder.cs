using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POs_Dashboard_Res_API.Models
{
    public class PendingOrder
    {
        public int Id { get; set; }
    public string? OrderNo { get; set; }
    public string? Table { get; set; }
    public string? WAITER { get; set; }
    public decimal Amount { get; set; }
    public int? OutletId { get; set; }
    }
}