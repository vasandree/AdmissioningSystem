using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Domain.Entities;

public class Manager
{
    [Key] public Guid ManagerId { get; set; }
    
    [Required] [ForeignKey("BaseManager")] public Guid MainId { get; set; }
    
    [Required] public Guid FacultyId { get; set; }
    
    [Required] public BaseManager BaseManager { get; set; }
}