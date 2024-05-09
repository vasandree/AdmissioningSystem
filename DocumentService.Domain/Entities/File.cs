using System.ComponentModel.DataAnnotations;

namespace DocumentService.Domain.Entities;

public class DbFile
{
    public Guid Id { get; set; }
    
    [Required]
    public string FileName { get; set; }
    
    [Required] 
    public byte[] FileContent { get; set; }
}