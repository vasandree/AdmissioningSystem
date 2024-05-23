using AutoMapper;
using Common.Models.Models.Dtos;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using DictionaryService.Application.Contracts.Persistence;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryService.Application.RpcHandler;

public class GetDtosHandler
{
    private readonly IBus _bus;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;
    
    public GetDtosHandler(IBus bus, IServiceProvider serviceProvider, IMapper mapper)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    public void CreateRequestListeners()
    {
        _bus.Rpc.RespondAsync<GetDocumentTypeDtoRequest, GetDocumentTypeDtoResponse >(async (request) =>
            await GetEducationDocumentTypeDto(request.DocumentTypeId));

        _bus.Rpc.RespondAsync<GetProgramDtoRequest, GetProgramDtoResponse>(async (request) =>
            await GetProgramDto(request.ProgramId));
    }

    private async Task<GetProgramDtoResponse> GetProgramDto(Guid programId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var programRepository = scope.ServiceProvider.GetRequiredService<IProgramRepository>();
            var program = await programRepository.GetById(programId);
            
            if (program.IsDeleted)
                return new GetProgramDtoResponse(null);

            return new GetProgramDtoResponse(_mapper.Map<ProgramDto>(program));
        }
    }

    private async Task<GetDocumentTypeDtoResponse> GetEducationDocumentTypeDto(Guid documentTypeId)
    {
        
        using (var scope = _serviceProvider.CreateScope())
        {
            var educationDoc = scope.ServiceProvider.GetRequiredService<IDocumentTypeRepository>();
            var nextEducationLevelsRepository = scope.ServiceProvider.GetRequiredService<INextEducationLevelRepository>();
            
            var documentType = await educationDoc.GetById(documentTypeId);

            if (documentType.IsDeleted)
                return new GetDocumentTypeDtoResponse(null);
            
            var nextEducationLevels = await nextEducationLevelsRepository.GetEducationLevels(documentType.Id);

            var dto = _mapper.Map<EducationDocumentTypeDto>(documentType);
            dto.NextEducationLevels = nextEducationLevels.Select(x => _mapper.Map<EducationLevelDto>(x)).ToList();


            return new GetDocumentTypeDtoResponse(dto);

        }
    }
}