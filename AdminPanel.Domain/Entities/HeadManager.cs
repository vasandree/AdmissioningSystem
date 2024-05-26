using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Domain.Entities;

public class HeadManager
{
    [Key] public Guid HeadManagerId { get; set; }

    [Required] [ForeignKey("BaseManager")] public Guid MainId { get; set; }

    [Required] public BaseManager BaseManager { get; set; }
}