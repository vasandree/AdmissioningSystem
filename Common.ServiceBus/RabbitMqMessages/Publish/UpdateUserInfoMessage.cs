using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Enums;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UpdateUserInfoMessage(
    Guid userId,
    string fullName,
    string? email,
    string? passwordHash,
    Gender? gender,
    string? nationality,
    DateTime? birthDate)
{
    public Guid UserId { get; } = userId;
    public string FullName { get; } = fullName;
    public string? Email { get; } = email;
    public string? PasswordHash { get; } = passwordHash;
    public Gender? Gender { get; } = gender;
    public string? Nationality { get; } = nationality;
    public DateTime? BirthDate { get; } = birthDate;
}
