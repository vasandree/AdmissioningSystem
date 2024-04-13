using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserApi.Domain.Enums;

namespace UserApi.Domain.DbEntities;

public class ApplicantEntity
{
    [Key] public Guid Id { get; set; }

    [Required] [ForeignKey("User")] public Guid UserId { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public Gender? Gender { get; set; }

    public string? Nationality { get; set; }

    public DateTime? BirthDate { get; set; }
}