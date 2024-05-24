using Common.Services.Repository;
using UserApi.Domain.DbEntities;

namespace UserService.Application.Contracts.Persistence;

public interface IApplicantRepository : IGenericRepository<ApplicantEntity>
{
    Task<ApplicantEntity?> GetByUserId(Guid id);
    
}