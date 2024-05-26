using Common.Models.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.ServiceBus.PubSub.Sender;
using DocumentService.Application.ServiceBus.RPC.RpcSender;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Commands.PassportInfo.EditPassportInfo;

public class EditPassportInfoCommandHandler : IRequestHandler<EditPassportInfoCommand, Unit>
{
    private readonly IDocumentRepository<Passport> _passport;
    private readonly RpcRequestSender _rpc;
    private readonly PubSubSender _pubSub;

    public EditPassportInfoCommandHandler(IDocumentRepository<Passport> passport, RpcRequestSender rpc, PubSubSender pubSub)
    {
        _passport = passport;
        _rpc = rpc;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(EditPassportInfoCommand request, CancellationToken cancellationToken)
    {
        if (await _rpc.CheckStatusClosed(request.UserId))
            throw new BadRequest("You cannot edit your profile, because one of your admissions is closed");

        
        if (await _passport.CheckExistence(request.UserId))
        {
            var passport = (Passport)(await _passport.GetByUserId(request.UserId))!;
            
            if (passport.IssueDate == null || passport.IssuedBy == null ||
                passport.SeriesAndNumber == null || passport.DateOfBirth == null)
            {
                throw new BadRequest("Passport info for this user does not exist");
            }

            passport.SeriesAndNumber = request.PassportInfoRequest.SeriesAndNumber;
            passport.IssueDate = request.PassportInfoRequest.IssueDate;
            passport.IssuedBy = request.PassportInfoRequest.IssuedBy;
            passport.DateOfBirth = request.PassportInfoRequest.BirthDate;

            await _passport.UpdateAsync(passport);
        }
        else
        {
            throw new BadRequest("Passport info for this user does not exist");
        }

        _pubSub.UpdateStatus(request.UserId);

        
        return Unit.Value;
    }
}