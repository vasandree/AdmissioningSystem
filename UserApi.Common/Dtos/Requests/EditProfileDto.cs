using System.ComponentModel.DataAnnotations;
using UserApi.Common.Enums;
using UserApi.Dtos.CustomValidationAttributes;

namespace UserApi.Common.Dtos.Requests;

public class EditProfileDto
{
    [MinLength(1)]
    [MaxLength(100)]
    [Required(ErrorMessage = "FullName is required")]
    public string FullName { get; set; }
    
    public Gender Gender { get; set; }
    
    [Phone]
    public string PhoneNumber { get; set; }

    public string Nationality { get; set; }
    
    [Date]
    public DateTime BirthDate { get; set; }
}