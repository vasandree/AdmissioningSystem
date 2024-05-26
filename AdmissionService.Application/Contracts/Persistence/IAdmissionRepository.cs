using System.Collections;
using AdmissionService.Domain.Entities;
using Common.Models.Models.Dtos;
using Common.Services.Repository;

namespace AdmissionService.Application.Contracts.Persistence;

public interface IAdmissionRepository : IGenericRepository<Admission>
{
    public Task<Admission> GetById(Guid id);
    
    public Task<bool> CheckIfPriorityAvailable(Guid userId, int priority);

    public bool CheckIfNewPriorityIsAvailable(Guid userId, int priority);
    
    public Task<bool> CheckIfAdmissionExists(Guid admissionId);

    public Task<bool> CheckIfAdmissionBelongsToApplicant(Guid userId, Guid admissionId);
    
    public Task<bool> CheckIfAdmissionIsAvailable(Guid userId);
    
    public bool CheckIfEducationLevelIsAvailable(ProgramDto programDto,
        EducationDocumentTypeDto educationDocumentTypeDto);

    public Task<bool> CheckIfEducationStageIsAvailable(Guid userId, ProgramDto programDto,
        EducationDocumentTypeDto educationDocumentTypeDto);
    
    public Task<List<Admission>> GetApplicantsAdmissions(Guid userId);
    Task<List<Admission>> GetAdmissionsByProgramIds(List<Guid> programsToDelete);

    Task<List<Admission>> GetAdmissionsByProgramId(Guid programId);
    bool CheckIfProgramIsChosen(Guid userId, Guid programId);
    Task<bool> CheckClosed(Guid userId);
    IQueryable<Admission> GetAsQueryable();
}