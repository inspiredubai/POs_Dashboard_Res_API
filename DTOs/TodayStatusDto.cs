public class TodayStatusDto
{
    public int Id { get; set; }
    public decimal CashAmount { get; set; }
    public decimal CreditCardAmount { get; set; }
    public decimal SalesAmount { get; set; }
    public decimal LastBillAmount { get; set; }
    public DateTime LastBillTime { get; set; }
    public string LastBillTimeString => LastBillTime.ToString("HH:mm:ss");
    public int TotalBills { get; set; }
    public int NoOfCustomers { get; set; }
}
