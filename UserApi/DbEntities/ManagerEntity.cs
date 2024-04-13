using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.DAL.DbEntities;

public class ManagerEntity
{ 
    [Key] 
    public Guid Id { get; set; }

    [Required]
    [ForeignKey("User")] 
    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;
    
    public Guid? Faculty { get; set; }
}
