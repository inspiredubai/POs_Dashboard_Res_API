public class TransactionDto
{
    public int Id { get; set; }
    public string? BillNo { get; set; } 
    public decimal Amount { get; set; }
    public decimal Cash { get; set; }
    public decimal Credit { get; set; }
    public string? CreatedDateString { get; set; }
    public int OutletId { get; set; }
}
