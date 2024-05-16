using AdmissionService.Domain.Entities;
using Common.Models.Dtos;

namespace AdmissionService.Application.Contracts.Persistence;

public interface IAdmissionRepository : IGenericRepository<Admission>
{
    public Task<bool> CheckIfAdmissionExists(Guid admissionId);
    
    public Task<bool> CheckIfAdmissionIsAvailable(Guid userId);

    public Task<bool> CheckIfAdmissionRefersToApplicant(Guid admissionId, Guid applicantId);

    public bool CheckIfEducationLevelIsAvailable(ProgramDto programDto,
        EducationDocumentTypeDto educationDocumentTypeDto);

    public Task<List<Admission>> GetApplicantsAdmissions(Guid userId);
}