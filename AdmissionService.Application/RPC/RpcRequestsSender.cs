using Common.Models.Models.Dtos;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;

namespace AdmissionService.Application.RPC;

public class RpcRequestsSender
{
    private readonly IBus _bus;

    public RpcRequestsSender(IBus bus)
    {
        _bus = bus;
    }
    
    
    public async Task<Guid?> CheckIfApplicantHasDocument(Guid applicantId)
    {
        var response = await _bus.Rpc.RequestAsync<EducationDocumentRequest, EducationDocumentResponse>(
            new EducationDocumentRequest(applicantId));
        
        return response.DocumentTypeId;
    }


    public async Task<EducationDocumentTypeDto?> GetEducationDocument(Guid documentTypeId)
    {
        var response = await 
            _bus.Rpc.RequestAsync<GetEducationDocumentTypeRequest, GetEducationDocumentResponse>(
                new GetEducationDocumentTypeRequest(documentTypeId));

        return response.EducationDocumentTypeDto;
    }

    public async Task<ProgramDto?> GetProgram(Guid programId)
    {
        var response =
            await _bus.Rpc.RequestAsync<GetProgramDtoRequest, GetProgramDtoResponse>(
                 new GetProgramDtoRequest(programId));
        return response.ProgramDto;
    }
}