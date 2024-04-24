using AutoMapper;
using MediatR;
using UserApi.Application.Contracts.Persistence;
using UserApi.Application.Dtos.Responses;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Queries.GetByEmail;

public class GetByEmailHandler : IRequestHandler<GetByEmailQuery, UserDto>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public GetByEmailHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetByEmailQuery request, CancellationToken cancellationToken)
    {
        var user =  await _repository.GetByEmail(request.Email);
        return _mapper.Map<UserDto>(user);
    }
}