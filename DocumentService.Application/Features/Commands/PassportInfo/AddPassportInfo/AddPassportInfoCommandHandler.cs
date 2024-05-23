using Common.Models.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Dtos.Requests;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using MediatR;

namespace DocumentService.Application.Features.Commands.PassportInfo.AddPassportInfo;

public class AddPassportInfoCommandHandler : IRequestHandler<AddPassportInfoCommand, Unit>
{
    private readonly IDocumentRepository<Passport> _passport;

    public AddPassportInfoCommandHandler(IDocumentRepository<Passport> passport)
    {
        _passport = passport;
    }

    public async Task<Unit> Handle(AddPassportInfoCommand request, CancellationToken cancellationToken)
    {
        if (await _passport.CheckExistence(request.UserId))
        {
            await ModifyExistingEntity(request.PassportInfo, request.UserId);
        }
        else
        {
            await CreateNewPassportEntity(request.PassportInfo, request.UserId);
        }

        return Unit.Value;
    }

    private async Task CreateNewPassportEntity(PassportInfoRequest passportInfoRequest, Guid userId)
    {
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
            throw new BadRequest("You have already added your passport info");
        }
        
        existingPassport.SeriesAndNumber = passportInfoRequest.SeriesAndNumber;
        existingPassport.DateOfBirth = passportInfoRequest.BirthDate;
        existingPassport.IssueDate = passportInfoRequest.IssueDate;
        existingPassport.IssuedBy = passportInfoRequest.IssuedBy;

        await _passport.UpdateAsync(existingPassport);
    }
}