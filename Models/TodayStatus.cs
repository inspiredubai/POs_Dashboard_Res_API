public class TodayStatus
{
    public int Id { get; set; }
    public decimal CashAmount { get; set; }
    public decimal CreditCardAmount { get; set; }
    public decimal SalesAmount { get; set; }
    public decimal LastBillAmount { get; set; }
    public DateTime LastBillTime { get; set; }
    public int? OutletId { get; set; }
    public int TotalBills { get; set; }
    public int NoOfCustomers { get; set; }

    public OutletMaster? Outlet { get; set; } // Navigation property (optional)
}
