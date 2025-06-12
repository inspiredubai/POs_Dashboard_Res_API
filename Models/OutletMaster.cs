public class OutletMaster
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool Status { get; set; }        // FIXED: was string?
    public int UserId { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }      // FIXED: was string?
    public int ModifiedBy { get; set; }     // FIXED: was string?
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
