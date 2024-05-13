using AutoMapper;
using MediatR;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Features.Queries.GetByEmail;

public class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public GetUserByIdCommandHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user =  await _repository.GetById(request.Id);
        return _mapper.Map<UserDto>(user);
    }
}