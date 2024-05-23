using System.ComponentModel.DataAnnotations;
using Common.Models.Models.ValidationAttributes;
using UserApi.Domain.Enums;

namespace UserService.Application.Dtos.Requests;

public class EditProfileDto
{
    [MinLength(1)]
    [MaxLength(100)]
    [Required(ErrorMessage = "FullName is required")]
    public string FullName { get; set; }
    
    public Gender? Gender { get; set; }

    public string? Nationality { get; set; }
    
    [DateNotInFuture]
    public DateTime? BirthDate { get; set; }
}