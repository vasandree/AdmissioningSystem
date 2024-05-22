using Common.Repository;
using UserApi.Domain.DbEntities;

namespace UserService.Application.Contracts.Persistence;

public interface IApplicantRepository : IGenericRepository<ApplicantEntity>
{
    Task<ApplicantEntity?> GetById(Guid id);
    
}