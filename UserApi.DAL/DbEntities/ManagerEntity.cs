using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.DAL.DbEntities;

public class ManagerEntity
{
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required] [ForeignKey("UserId")] public ApplicationUser User { get; set; } = null!;
    
    public Guid Faculty { get; set; } 
}