using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserApi.DAL.DbEntities;

namespace UserApi.DAL;

public sealed class UserDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{

    public  UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ApplicationUser>()
            .HasOne(x => x.Student)
            .WithOne(x => x.User)
            .HasForeignKey<StudentEntity>().IsRequired();
        
        modelBuilder.Entity<ApplicationUser>()
            .HasOne(x => x.Manager)
            .WithOne(x => x.User)
            .HasForeignKey<ManagerEntity>().IsRequired();
        
    }
    public DbSet<StudentEntity> Students { get; set; } = null!;
    public DbSet<ManagerEntity> Managers { get; set; } = null!;
}