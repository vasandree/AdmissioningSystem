using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Dtos;

namespace AdminPanel.MVC.Models;

public class ProfileViewModel
{
    public required Guid Id { get; set; }
    [MinLength(5)]
    public required string FullName { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public required FacultyDto? Faculty { get; set; }
}