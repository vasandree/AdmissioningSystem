using UserApi.Domain.DbEntities;

namespace UserApi.Application.Contracts.Persistence;

public interface IApplicantRepository : IGenericRepository<Domain.DbEntities.ApplicantEntity>
{
    Task<ApplicantEntity?> GetById(Guid id);
    
}