using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Domain.DbEntities;

public class ApplicantEntity
{
    [Key] public Guid Id { get; set; }

    [Required] [ForeignKey("User")] public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;

}