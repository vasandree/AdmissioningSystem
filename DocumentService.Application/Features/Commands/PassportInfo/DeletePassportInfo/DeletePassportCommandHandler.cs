using Common.Models.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.ServiceBus.PubSub.Sender;
using DocumentService.Application.ServiceBus.RPC.RpcSender;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Commands.PassportInfo.DeletePassportInfo;

public class DeletePassportCommandHandler : IRequestHandler<DeletePassportInfoCommand, Unit>
{
    private readonly IDocumentRepository<Passport> _passport;
    private readonly RpcRequestSender _rpc;
    private readonly PubSubSender _pubSub;

    public DeletePassportCommandHandler(IDocumentRepository<Passport> passport, RpcRequestSender rpc, PubSubSender pubSub)
    {
        _passport = passport;
        _rpc = rpc;
        _pubSub = pubSub;
    }


    public async Task<Unit> Handle(DeletePassportInfoCommand request, CancellationToken cancellationToken)
    {
        if (await _rpc.CheckStatusClosed(request.UserId))
            throw new BadRequest("You cannot edit your profile, because one of your admissions is closed");

        
        if (!await _passport.CheckExistence(request.UserId))
        {
            throw new BadRequest("PassportInfo for this user does not exist");
        }

        var passport = (Passport)(await _passport.GetByUserId(request.UserId))!;

        if (passport.File == null)
        {
            await _passport.DeleteAsync(passport);
        }
        else
        {
            passport.IssueDate = null;
            passport.IssuedBy = null;
            passport.DateOfBirth = null;
            passport.SeriesAndNumber = null;

            await _passport.UpdateAsync(passport);
        }
        
        _pubSub.UpdateStatus(request.UserId);


        return Unit.Value;
    }
}