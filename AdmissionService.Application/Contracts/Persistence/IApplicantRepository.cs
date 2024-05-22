using AdmissionService.Domain.Entities;
using Common.Repository;

namespace AdmissionService.Application.Contracts.Persistence;

public interface IApplicantRepository : IGenericRepository<Applicant>
{
    public Task<bool> CheckIfApplicantExists(Guid userId);

    public Task<Applicant> GetById(Guid id);
}