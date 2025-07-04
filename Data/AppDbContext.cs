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

}

