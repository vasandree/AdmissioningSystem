using Microsoft.AspNetCore.Identity;

namespace UserApi.DAL.DbEntities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; }
    
    public ApplicantEntity? Student { get; set; }
    
    public ManagerEntity? Manager { get; set; }
    
}
