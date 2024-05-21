using Common.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Commands.PassportInfo.DeletePassportInfo;

public class DeletePassportCommandHandler : IRequestHandler<DeletePassportInfoCommand, Unit>
{
    private readonly IDocumentRepository<Passport> _passport;

    public DeletePassportCommandHandler(IDocumentRepository<Passport> passport)
    {
        _passport = passport;
    }


    public async Task<Unit> Handle(DeletePassportInfoCommand request, CancellationToken cancellationToken)
    {
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

        return Unit.Value;
    }
}