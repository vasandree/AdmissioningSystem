namespace Common.ServiceBus.RabbitMqMessages.Request;

public class EducationDocumentInfoRequest(Guid applicantId)
{
    public Guid ApplicantId { get; set; } = applicantId;
}