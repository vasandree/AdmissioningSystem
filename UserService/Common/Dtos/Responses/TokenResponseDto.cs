using System.ComponentModel.DataAnnotations;

namespace UserService.Common.Dtos.Responses;

public class TokenResponseDto
{
    [Required]
    [MinLength(1)]
    public string Token { get; set; }
}