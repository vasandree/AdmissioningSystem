using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Domain.Entities;
using AdmissionService.Infrastructure;
using Common.Models.Models.Dtos;
using Common.Models.Models.Enums;
using Common.Services.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AdmissionService.Persistence.Repositories;

public class AdmissionRepository : GenericRepository<Admission>, IAdmissionRepository
{
    private readonly AdmissionDbContext _context;
    private readonly IConfiguration _config;

    public AdmissionRepository(AdmissionDbContext context, IConfiguration config) : base(context)
    {
        _context = context;
        _config = config;
    }


    public async Task<Admission> GetById(Guid id)
    {
        return (await _context.Admissions.FirstOrDefaultAsync(x => x.AdmissionId == id))!;
    }

    public async Task<bool> CheckIfPriorityAvailable(Guid userId, int priority)
    {
        var admissions = await _context.Admissions.Where(x => x.ApplicantId == userId).ToListAsync();

        return admissions.Count == 0 && priority == 0;
    }

    public bool CheckIfNewPriorityIsAvailable(Guid userId, int priority)
    {
        var admissionsAmount = _context.Admissions.Count(x => x.ApplicantId == userId);
        return priority < admissionsAmount;
    }

    public async Task<bool> CheckIfAdmissionExists(Guid admissionId)
    {
        return await _context.Admissions.AnyAsync(x => x.AdmissionId == admissionId);
    }

    public async Task<bool> CheckIfAdmissionBelongsToApplicant(Guid userId, Guid admissionId)
    {
        var admission = await _context.Admissions.FirstOrDefaultAsync(x => x.AdmissionId == admissionId);
        return admission!.ApplicantId == userId;
    }


    public async Task<bool> CheckIfAdmissionIsAvailable(Guid userId)
    {
        var admissions = await _context.Admissions.Where(x => x.ApplicantId == userId).ToListAsync();
        if (admissions.Count >= _config.GetValue<int>("MaxAdmissionsAmount")) return false;
        return true;
    }

    public bool CheckIfEducationLevelIsAvailable(ProgramDto programDto,
        EducationDocumentTypeDto educationDocumentTypeDto)
    {
        if (programDto.EducationLevel.Id == educationDocumentTypeDto.EducationLevel.Id || (
                educationDocumentTypeDto.NextEducationLevels != null &&
                educationDocumentTypeDto.NextEducationLevels.Any(level => programDto.EducationLevel.Id == level.Id)))
        {
            return true;
        }

        return false;
    }

    public async Task<bool> CheckIfEducationStageIsAvailable(Guid userId, ProgramDto programDto,
        EducationDocumentTypeDto educationDocumentTypeDto)
    {
        var nextAvailableLevelsIds = educationDocumentTypeDto.NextEducationLevels?.Select(x => x.Id).ToList();

        if (nextAvailableLevelsIds == null)
        {
            return programDto.EducationLevel.Id == educationDocumentTypeDto.EducationLevel.Id;
        }

        var chosenLevelsIds = await _context.Admissions
            .Where(x => x.ApplicantId == userId && !x.IsDeleted)
            .Select(x => x.EducationLevelId)
            .Distinct()
            .ToListAsync();

        if (nextAvailableLevelsIds.Contains(programDto.EducationLevel.Id) &&
            programDto.EducationLevel.Id != educationDocumentTypeDto.EducationLevel.Id)
            return true;

        if (!nextAvailableLevelsIds.Contains(programDto.EducationLevel.Id) &&
            programDto.EducationLevel.Id == educationDocumentTypeDto.EducationLevel.Id)
            return true;

        return false;
    }


    public async Task<List<Admission>> GetApplicantsAdmissions(Guid userId)
    {
        var admissions = await _context.Admissions
            .Where(x => x.ApplicantId == userId && !x.IsDeleted)
            .OrderBy(x => x.Priority)
            .ToListAsync();

        return admissions;
    }

    public async Task<List<Admission>> GetAdmissionsByProgramIds(List<Guid> programsToDelete)
    {
        return await _context.Admissions
            .Where(admission => programsToDelete
                .Contains(admission.ProgramId) && !admission.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<Admission>> GetAdmissionsByProgramId(Guid programId)
    {
        return await _context.Admissions.Where(admission => admission.ProgramId == programId).ToListAsync();
    }

    public bool CheckIfProgramIsChosen(Guid userId, Guid programId)
    {
        return _context.Admissions.Any(x => x.ApplicantId == userId && x.ProgramId == programId);
    }

    public Task<bool> CheckClosed(Guid userId)
    {
        return _context.Admissions.AnyAsync(x => x.ApplicantId == userId && x.Status == AdmissionStatus.Closed);
    }

    public IQueryable<Admission> GetAsQueryable()
    {
        return _context.Admissions.AsQueryable();
    }
}