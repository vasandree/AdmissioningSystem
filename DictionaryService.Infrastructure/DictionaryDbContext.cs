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
    
    public DbSet<NextEducationLevel> NextEducationLevels { get; set; }
    
  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
