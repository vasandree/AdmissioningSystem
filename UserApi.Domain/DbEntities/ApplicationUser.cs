using Microsoft.AspNetCore.Identity;
using UserApi.Domain.Enums;

namespace UserApi.Domain.DbEntities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; }

    public string? ConfirmCode { get; set; }

    public ApplicantEntity? Student { get; set; }

    public ManagerEntity? Manager { get; set; }
    
    public Gender? Gender { get; set; }

    public string? Nationality { get; set; }

    public DateTime? BirthDate { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}