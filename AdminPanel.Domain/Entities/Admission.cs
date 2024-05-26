using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Domain.Entities;

public class Admission
{
    [Key] public Guid Id { get; set; }
    
    public Guid? ManagerId { get; set; }
}