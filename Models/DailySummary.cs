public class DailySummary
{
    public int Id { get; set; }
    public decimal CashAmount { get; set; }
    public decimal CreditCardAmount { get; set; }
    public decimal SalesAmount { get; set; }
    public int TotalBills { get; set; }
    public DateTime SummaryDate { get; set; }
    public int OutletId { get; set; }
}
