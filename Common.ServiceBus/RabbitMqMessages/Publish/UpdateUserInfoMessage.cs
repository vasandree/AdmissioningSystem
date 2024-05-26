using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Enums;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UpdateUserInfoMessage(
    Guid userId,
    string fullName,

    Gender? gender,
    string? nationality,
    DateTime? birthDate)
{
    public Guid UserId { get; } = userId;
    public string FullName { get; } = fullName;
    public Gender? Gender { get; } = gender;
    public string? Nationality { get; } = nationality;
    public DateTime? BirthDate { get; } = birthDate;
}
