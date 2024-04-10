using System.ComponentModel.DataAnnotations;
using UserService.Common.Enums;

namespace UserService.Common.Dtos.Requests;

public class RoleChangeDto
{
    [Required(ErrorMessage = "Role is required")]
    public Role Role { get; set; }
}