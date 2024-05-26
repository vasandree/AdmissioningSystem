using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Exceptions;
using Common.Models.Models.Enums;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.UserDocuments.EditPassport;

public class EditUserPassportCommandHandler : IRequestHandler<EditUserPassportCommand, Unit>
{
    private readonly RpcRequestSender _rpc;
    private readonly IManagerRepository _manager;
    private readonly IBaseManagerRepository _repository;
    private readonly PubSubSender _pubSub;

    public EditUserPassportCommandHandler(RpcRequestSender rpc, IManagerRepository manager,
        IBaseManagerRepository repository, PubSubSender pubSub)
    {
        _rpc = rpc;
        _manager = manager;
        _repository = repository;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(EditUserPassportCommand request, CancellationToken cancellationToken)
    {
        var manager = await _repository.GetById(request.ManagerId);

        if (manager.Id == request.UserId)
            throw new BadRequest("You are not allowed to edit your documents as manager");
        

        if (!await _rpc.CheckDocumentExistence(request.UserId, DocumentType.Passport))
            throw new BadRequest("Provided user does not have education doc");


        if (await _repository.CheckIfManager(manager))
        {
            if (await _rpc.CheckManagersApplicant(request.ManagerId, request.UserId)) 
                throw new Forbidden("You are not a manager of this applicant");

        }

        _pubSub.EditUserPassport(request.UserId, request.SeriesAndNumber, request.IssuedBy, request.DateOfBirth,
            request.IssueDate);

        return Unit.Value;
    }
}