using System.ComponentModel.DataAnnotations;

namespace DocumentService.Domain.Entities;

public class File
{
    public Guid Id { get; set; }
    
    [Required] 
    public byte[] FileContent { get; set; }
}