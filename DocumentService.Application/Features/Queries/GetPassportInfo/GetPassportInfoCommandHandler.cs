using AutoMapper;
using Common.Models.Exceptions;
using Common.Models.Models.Dtos;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Queries.GetPassportInfo;

public class GetPassportInfoCommandHandler : IRequestHandler<GetPassportInfoCommand, PassportInfoDto>
{
    private readonly IDocumentRepository<Passport> _passport;
    private readonly IMapper _mapper;

    public GetPassportInfoCommandHandler(IDocumentRepository<Passport> passport, IMapper mapper)
    {
        _passport = passport;
        _mapper = mapper;
    }

    public async Task<PassportInfoDto> Handle(GetPassportInfoCommand request, CancellationToken cancellationToken)
    {
        if (!await _passport.CheckExistence(request.UserId))
        {
            throw new BadRequest("There is no passport info for this user");
        }

        var passportInfo = (Passport) (await _passport.GetByUserId(request.UserId))!;
        
        if (passportInfo.IssueDate == null || passportInfo.IssuedBy == null ||
            passportInfo.SeriesAndNumber == null || passportInfo.DateOfBirth == null)
        {
            throw new BadRequest("Passport info for this user does not exist");
        }

        return _mapper.Map<PassportInfoDto>(passportInfo);
    }
}