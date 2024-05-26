using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UpdatePassportMessage(Guid userId, string seriesAndNumber, string issuedBy, DateTime dateOfBirth, DateTime issueDate)
{
        [Required] public Guid UserId { get; set; } = userId;
        [Required] public string SeriesAndNumber { get; set; } = seriesAndNumber;
        [Required] public string IssuedBy { get; set; } = issuedBy;
        [Required] public DateTime DateOfBirth { get; set; } = dateOfBirth;
        [Required] public DateTime IssueDate { get; set; } = issueDate;
}