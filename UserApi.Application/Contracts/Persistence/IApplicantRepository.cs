using UserApi.Domain.DbEntities;

namespace UserApi.Application.Contracts.Persistence;

public interface IApplicantRepository : IGenericRepository<ApplicantEntity>
{
    Task<ApplicantEntity?> GetById(Guid id);
    
}