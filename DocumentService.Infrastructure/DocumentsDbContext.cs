using DocumentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Infrastructure;

public class DocumentsDbContext : DbContext
{
    public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<EducationDocument> EducationDocuments { get; set; }
    public DbSet<Passport> Passports { get; set; }
    public DbSet<DbFile?> Files { get; set; }
    
    
}