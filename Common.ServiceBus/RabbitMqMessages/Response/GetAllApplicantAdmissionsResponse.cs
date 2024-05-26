using System.ComponentModel.DataAnnotations;
using Common.Models.Models;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetAllApplicantAdmissionsResponse(List<AdmissionDto> admissions, Exception? exceptionToThrow = null)
    : BaseResponse(exceptionToThrow)
{
    [Required] public List<AdmissionDto> Admissions { get; set; } = admissions;
}