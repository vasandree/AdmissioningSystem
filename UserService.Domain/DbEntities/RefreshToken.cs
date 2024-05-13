using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserApi.Domain.DbEntities;

public class RefreshToken
{
    [Required] 
    [ForeignKey("User")] 
    public Guid UserId { get; set; }
    
    [Required]
    [Key]
    public string Token { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }
    
    public ApplicationUser User { get; set; }
}