using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Exceptions;
using Common.Models.Models.Enums;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.UserFiles.DeleteFile;

public class DeleteUserFileCommandHandler : IRequestHandler<DeleteUserFileCommand, Unit>
{
    private readonly RpcRequestSender _rpc;
    private readonly PubSubSender _pubSub;
    private readonly IManagerRepository _manager;
    private readonly IBaseManagerRepository _repository;

    public DeleteUserFileCommandHandler(RpcRequestSender rpc, PubSubSender pubSub, IManagerRepository manager,
        IBaseManagerRepository repository)
    {
        _rpc = rpc;
        _pubSub = pubSub;
        _manager = manager;
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteUserFileCommand request, CancellationToken cancellationToken)
    {
        var manager = await _repository.GetById(request.ManagerId);

        if (manager.Id == request.UserId)
            throw new BadRequest("You are not allowed to edit your documents as manager");
        

        if (!await _rpc.CheckDocumentExistence(request.UserId, DocumentType.EducationDocument, true))
            throw new BadRequest("Provided user does not have file to delete");


        if (await _repository.CheckIfManager(manager))
        {
            if (await _rpc.CheckManagersApplicant(request.ManagerId, request.UserId)) 
                throw new Forbidden("You are not a manager of this applicant");

        }

        _pubSub.DeleteFile(request.UserId, request.DocumentType);

        return Unit.Value;
    }
}