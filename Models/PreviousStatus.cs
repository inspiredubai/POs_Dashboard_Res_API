using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POs_Dashboard_Res_API.Models
{
    public class PreviousStatus
    {
        public int Id { get; set; }
    public decimal CashAmount { get; set; }
    public decimal CreditCardAmount { get; set; }
    public decimal SalesAmount { get; set; }
    public int? OutletId { get; set; }
    }
}