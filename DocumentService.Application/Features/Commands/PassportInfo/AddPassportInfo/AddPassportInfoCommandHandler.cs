using Common.Models.Exceptions;
using Common.Models.Models.Dtos;
using Common.Models.Models.Enums;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Dtos.Requests;
using DocumentService.Application.ServiceBus.PubSub.Sender;
using DocumentService.Application.ServiceBus.RPC.RpcSender;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Commands.PassportInfo.AddPassportInfo;

public class AddPassportInfoCommandHandler : IRequestHandler<AddPassportInfoCommand, Unit>
{
    private readonly IDocumentRepository<Passport> _passport;
    private readonly RpcRequestSender _rpc;
    private readonly PubSubSender _pubSub;

    public AddPassportInfoCommandHandler(IDocumentRepository<Passport> passport, RpcRequestSender rpc, PubSubSender pubSub)
    {
        _passport = passport;
        _rpc = rpc;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(AddPassportInfoCommand request, CancellationToken cancellationToken)
    {
        if (await _rpc.CheckStatusClosed(request.UserId))
            throw new BadRequest("You cannot edit your profile, because one of your admissions is closed");

        
        if (await _passport.CheckExistence(request.UserId))
        {
            await ModifyExistingEntity(request.PassportInfo, request.UserId);
        }
        else
        {
            await CreateNewPassportEntity(request.PassportInfo, request.UserId);
        }

        _pubSub.UpdateStatus(request.UserId);

        
        return Unit.Value;
    }

    private async Task CreateNewPassportEntity(PassportInfoRequest passportInfoRequest, Guid userId)
    {
        var document = (Passport)await _passport.GetByUserId(userId);
        if (!document.IsDeleted || document.SeriesAndNumber != null || document.DateOfBirth != null || document.IssueDate != null || 
            document.IssuedBy != null)
        {
            throw new Conflict("You have already added education document info");
        }
        
        await _passport.CreateAsync(new Passport
        {
            Id = Guid.NewGuid(),
            DocumentType = DocumentType.Passport,
            UserId = userId,
            File = null,
            SeriesAndNumber = passportInfoRequest.SeriesAndNumber,
            DateOfBirth = passportInfoRequest.BirthDate,
            IssueDate = passportInfoRequest.IssueDate,
            IssuedBy = passportInfoRequest.IssuedBy
        });
    }

    private async Task ModifyExistingEntity(PassportInfoRequest passportInfoRequest, Guid userId)
    {
        var existingPassport = (Passport)(await _passport.GetByUserId(userId))!;

        if (existingPassport.IssueDate != null || existingPassport.IssuedBy != null || 
            existingPassport.SeriesAndNumber != null || existingPassport.DateOfBirth != null)
        {
            throw new Conflict("You have already added your passport info");
        }
        
        existingPassport.SeriesAndNumber = passportInfoRequest.SeriesAndNumber;
        existingPassport.DateOfBirth = passportInfoRequest.BirthDate;
        existingPassport.IssueDate = passportInfoRequest.IssueDate;
        existingPassport.IssuedBy = passportInfoRequest.IssuedBy;

        await _passport.UpdateAsync(existingPassport);
    }
}