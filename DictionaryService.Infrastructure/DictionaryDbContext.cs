using DictionaryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DictionaryService.Infrastructure;

using Microsoft.EntityFrameworkCore;

public class DictionaryDbContext : DbContext
{
    public DictionaryDbContext(DbContextOptions<DictionaryDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<DocumentType> DocumentTypes { get; set; }
    public DbSet<EducationLevel> EducationLevels { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Program> Programs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Program>()
            .HasOne(p => p.EducationLevel)
            .WithMany()
            .HasForeignKey(p => p.EducationLevelId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Program>()
            .HasOne(p => p.Faculty)
            .WithMany()
            .HasForeignKey(p => p.FacultyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DocumentType>()
            .HasOne(dt => dt.EducationLevel)
            .WithMany()
            .HasForeignKey(dt => dt.EducationLevelId)
            .OnDelete(DeleteBehavior.Cascade);

    }


}
