using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserApi.Domain.DbEntities;

namespace UserApi.Infrastructure;

public sealed class UserDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicantEntity> Applicants { get; set; } = null!;
    public DbSet<ManagerEntity> Managers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(x => x.Student)
            .WithOne(x => x.User)
            .HasForeignKey<ApplicantEntity>().IsRequired();

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(x => x.Manager)
            .WithOne(x => x.User)
            .HasForeignKey<ManagerEntity>().IsRequired();
    }
}