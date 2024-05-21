using AdmissionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdmissionService.Infrastructure;

public class AdmissionDbContext : DbContext
{
    public AdmissionDbContext(DbContextOptions<AdmissionDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Admission> Admissions { get; set; }
    public DbSet<Applicant> Applicants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Admission>()
            .HasOne(a => a.Applicant)
            .WithMany(u => u.Admissions)
            .HasForeignKey(rt => rt.ApplicantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}