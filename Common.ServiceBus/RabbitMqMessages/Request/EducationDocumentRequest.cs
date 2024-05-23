namespace Common.ServiceBus.RabbitMqMessages.Request;

public class EducationDocumentRequest(Guid applicantId)
{
    public Guid ApplicantId { get; set; } = applicantId;
}