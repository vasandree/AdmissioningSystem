namespace Common.ServiceBus.RabbitMqMessages.Request;

public class EducationDocRequest(Guid applicantId)
{
    public Guid ApplicantId { get; set; } = applicantId;
}