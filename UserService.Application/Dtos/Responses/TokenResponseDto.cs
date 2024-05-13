using System.ComponentModel.DataAnnotations;

namespace UserService.Application.Dtos.Responses;

public class TokenResponseDto
{
    [Required]
    [MinLength(1)]
    public string AcccessToken { get; set; }
    
    [Required]
    [MinLength(1)]
    public string RefreshToken { get; set; }
}