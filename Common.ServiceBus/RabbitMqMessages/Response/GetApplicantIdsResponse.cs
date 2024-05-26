using System.ComponentModel.DataAnnotations;


namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetApplicantIdsResponse(List<Guid> applicantIds, Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{ 
    [Required] public List<Guid> ApplicantIds { get; set; } = applicantIds;
}