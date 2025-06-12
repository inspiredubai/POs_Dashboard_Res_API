using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PosApi.Models;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<OutletMaster> OutletMasters { get; set; }
    public DbSet<GroupSummary> GroupSummaries { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<DailySummary> DailySummaries { get; set; }



}

