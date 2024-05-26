using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Domain.Entities;

public class BaseManager :  IdentityUser<Guid>
{
    [Required]
    public Guid UserId { get; set; }
        
    [Required]
    public string FullName { get; set; }
    public Manager? Manager { get; set; }
    
    public HeadManager? HeadManager { get; set; }
    
}