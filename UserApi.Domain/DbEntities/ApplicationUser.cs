using Microsoft.AspNetCore.Identity;

namespace UserApi.Domain.DbEntities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; }

    public string? ConfirmCode { get; set; }

    public ApplicantEntity? Student { get; set; }

    public ManagerEntity? Manager { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiryDate { get; set; }
}