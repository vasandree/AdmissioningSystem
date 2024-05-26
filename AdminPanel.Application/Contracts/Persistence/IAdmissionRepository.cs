using AdminPanel.Domain.Entities;
using Common.Services.Repository;

namespace AdminPanel.Application.Contracts.Persistence;

public interface IAdmissionRepository : IGenericRepository<Admission>
{
    Task<bool> CheckExistence(Guid admissionId);
    Task<Admission> GetById(Guid admissionId);

    Task DeleteManager(Guid admissionId);
}