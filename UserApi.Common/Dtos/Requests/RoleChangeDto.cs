using System.ComponentModel.DataAnnotations;
using UserApi.Common.Enums;

namespace UserApi.Common.Dtos.Requests;

public class RoleChangeDto
{
    [Required(ErrorMessage = "Role is required")]
    public Role Role { get; set; }
}