using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    
    [ForeignKey("FileId")]
    public File? File { get; set; }
}