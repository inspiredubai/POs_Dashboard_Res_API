public class Transaction
{
    public int Id { get; set; }
    public string? BillNo { get; set; }
    public decimal Amount { get; set; }
    public decimal Cash { get; set; }
    public decimal Credit { get; set; }
    public int OutletId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public int? CreatedBy { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
