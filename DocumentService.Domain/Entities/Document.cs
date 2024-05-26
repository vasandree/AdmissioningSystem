using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models.Models.Enums;

namespace DocumentService.Domain.Entities;

public abstract class Document
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public DocumentType DocumentType { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey("FileId")]
    public DbFile? File { get; set; }
    
    public bool IsDeleted { get; set; }
}