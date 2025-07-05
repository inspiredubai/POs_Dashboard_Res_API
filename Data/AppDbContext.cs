using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POs_Dashboard_Res_API.Models;
using PosApi.Models;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<PendingOrder> PendingOrders { get; set; }
    public DbSet<OutletMaster> OutletMasters { get; set; }
    public DbSet<GroupSummary> GroupSummaries { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<DailySummary> DailySummaries { get; set; }
    public DbSet<VoidTransaction> VoidTransactions { get; set; }
    public DbSet<PreviousStatus> PreviousStatuses { get; set; }
    public DbSet<TodayStatus> TodayStatuses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mark VoidTransaction as keyless
        modelBuilder.Entity<VoidTransaction>(entity =>
        {
            entity.ToTable("Void_Transactions");
            entity.HasNoKey();
        });
        modelBuilder.Entity<PreviousStatus>().ToTable("PreviousStatus");
        // Optional: map PreviousStatus to table name if not matching class name
        //modelBuilder.Entity<PreviousStatus>().ToTable("PreviousStatus");

        // Optional: map other custom-named tables similarly
    }
}

