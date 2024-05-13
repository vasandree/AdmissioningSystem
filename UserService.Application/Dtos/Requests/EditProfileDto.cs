using System.ComponentModel.DataAnnotations;
using UserApi.Domain.Enums;
using UserService.Application.Dtos.CustomValidationAttributes;

namespace UserService.Application.Dtos.Requests;

public class EditProfileDto
{
    [MinLength(1)]
    [MaxLength(100)]
    [Required(ErrorMessage = "FullName is required")]
    public string FullName { get; set; }
    
    public Gender? Gender { get; set; }

    public string? Nationality { get; set; }
    
    [Date]
    public DateTime? BirthDate { get; set; }
}