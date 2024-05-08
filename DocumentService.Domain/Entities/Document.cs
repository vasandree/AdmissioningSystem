using System.ComponentModel.DataAnnotations;
using DocumentService.Domain.Enums;

namespace DocumentService.Domain.Entities;

public abstract class Document
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public DocumentType DocumentType { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    public File? File { get; set; }
}