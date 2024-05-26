using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Dtos.PagedDtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetAllAdmissionsResponse(AdmissionsPagedDto admissionsPagedDtos, Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{ 
    [Required] public AdmissionsPagedDto Admissions { get; set; } = admissionsPagedDtos;
}