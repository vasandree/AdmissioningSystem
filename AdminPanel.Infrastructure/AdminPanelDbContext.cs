using AdminPanel.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Infrastructure;

public class AdminPanelDbContext : IdentityDbContext<BaseManager, IdentityRole<Guid>, Guid>
{
    public AdminPanelDbContext(DbContextOptions<AdminPanelDbContext> options)
        : base(options)
    {
    }

    public DbSet<Manager> Managers { get; set; }
    
    public DbSet<HeadManager> HeadManagers { get; set; }
    
    public DbSet<Admission> Admissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BaseManager>()
            .HasOne(x => x.Manager)
            .WithOne(x => x.BaseManager)
            .HasForeignKey<Manager>(x => x.MainId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BaseManager>()
            .HasOne(x => x.HeadManager)
            .WithOne(x => x.BaseManager)
            .HasForeignKey<HeadManager>(x => x.MainId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}