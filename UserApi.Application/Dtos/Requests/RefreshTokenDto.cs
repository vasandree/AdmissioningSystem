using System.ComponentModel.DataAnnotations;

namespace UserApi.Application.Dtos.Requests;

public class RefreshTokenDto
{
    [Required]
    public string RefreshToken { get; set; }
    
    [Required]
    public string AccessToken { get; set; }
}