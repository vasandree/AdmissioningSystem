using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Exceptions;
using Common.Models.Models.Enums;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.UserDocuments.EditEducationDocument;

public class EditUserEducationDocumentCommandHandler : IRequestHandler<EditUserEducationDocumentCommand, Unit>
{
    private readonly RpcRequestSender _rpc;
    private readonly IManagerRepository _manager;
    private readonly IBaseManagerRepository _repository;
    private readonly PubSubSender _pubSub;

    public EditUserEducationDocumentCommandHandler(RpcRequestSender rpc, IBaseManagerRepository repository, PubSubSender pubSub, IManagerRepository manager)
    {
        _rpc = rpc;
        _repository = repository;
        _pubSub = pubSub;
        _manager = manager;
    }

    public async Task<Unit> Handle(EditUserEducationDocumentCommand request, CancellationToken cancellationToken)
    {
        var manager = await _repository.GetById(request.ManagerId);

        if (manager.Id == request.UserId)
            throw new BadRequest("You are not allowed to edit your documents as manager");
        
        
        if (!await _rpc.CheckDocumentExistence(request.UserId, DocumentType.EducationDocument))
            throw new BadRequest("Provided user does not have education doc");
        
        if (await _repository.CheckIfManager(manager))
        {
            if (await _rpc.CheckManagersApplicant(request.ManagerId, request.UserId)) 
                throw new Forbidden("You are not a manager of this applicant");

        }

        await _pubSub.EditUserEducationDocument(request.UserId, request.Name);
        
        return Unit.Value;
    }
}