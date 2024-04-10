using System.ComponentModel.DataAnnotations;

namespace UserService.Common.Dtos.Responses;

public class RolesDto
{
    [Required]
    public bool IsApplicant { get; set; }
    
    [Required]
    public bool IsManager { get; set; }
    
    [Required]
    public bool IsHeadManager { get; set; }
    
    [Required]
    public bool IsAdmin { get; set; }
}