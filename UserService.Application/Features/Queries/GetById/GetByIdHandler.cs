using MediatR;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.Features.Queries.GetById;

public class GetByIdHandler<T> : IRequestHandler<GetByIdQuery<T>, T> where T : class
{
    private readonly IUserRepository _userRepository;
    private readonly IManagerRepository _managerRepository;
    private readonly IApplicantRepository _applicantRepository;

    public GetByIdHandler(IUserRepository userRepository, IManagerRepository managerRepository,
        IApplicantRepository applicantRepository)
    {
        _userRepository = userRepository;
        _managerRepository = managerRepository;
        _applicantRepository = applicantRepository;
    }

    public async Task<T?> Handle(GetByIdQuery<T> request, CancellationToken cancellationToken)
    {
        if (typeof(T) == typeof(ApplicationUser))
        {
            if (request.Id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty", nameof(request.Id));

            return await _userRepository.GetById(request.Id) as T;
        }

        if (typeof(T) == typeof(ManagerEntity))
        {
            if (request.Id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty", nameof(request.Id));

            return await _managerRepository.GetById(request.Id) as T;
        }

        if (typeof(T) == typeof(ApplicantEntity))
        {
            if (request.Id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty", nameof(request.Id));

            return await _applicantRepository.GetByUserId(request.Id) as T;
        }

        throw new NotSupportedException($"Type '{typeof(T).Name}' is not supported.");
    }
}