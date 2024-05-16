using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Domain.Entities;
using AdmissionService.Infrastructure;
using Common.Models.Dtos;
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


    public async Task<bool> CheckIfAdmissionExists(Guid admissionId)
    {
        return await _context.Admissions.AnyAsync(x => x.AdmissionId == admissionId);
    }

    public async Task<bool> CheckIfAdmissionRefersToApplicant(Guid admissionId, Guid applicantId)
    {
        var admission = await _context.Admissions.FirstOrDefaultAsync(x => x.AdmissionId == admissionId);
        return admission!.ApplicantId == applicantId;
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


    public async Task<List<Admission>> GetApplicantsAdmissions(Guid userId)
    {
        return await _context.Admissions.Where(x => x.ApplicantId == userId).ToListAsync();
    }
}