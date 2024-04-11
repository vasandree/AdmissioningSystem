using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserApi.Common.Enums;

namespace UserApi.DAL.DbEntities;

public class ApplicantEntity
{
    public Guid Id { get; set; }
    
    [Required]
    [ForeignKey("UserId")] 
    public ApplicationUser User { get; set; } = null!;
    
    public Gender Gender { get; set; }
    
    public string Nationality { get; set; }
    
    public DateTime BirthDate { get; set; }
}
