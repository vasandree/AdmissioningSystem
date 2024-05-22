using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Domain.Entities;
using AdmissionService.Infrastructure;
using Common.Repository;
using Microsoft.EntityFrameworkCore;

namespace AdmissionService.Persistence.Repositories;

public class ApplicantRepository : GenericRepository<Applicant>, IApplicantRepository
{
    private readonly AdmissionDbContext _context;
    
    public ApplicantRepository(AdmissionDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<bool> CheckIfApplicantExists(Guid userId)
    {
        return await _context.Applicants.AnyAsync(x => x.ApplicantId == userId);
    }

    public async Task<Applicant> GetById(Guid id)
    {
        return await _context.Applicants.FirstOrDefaultAsync(x => x.ApplicantId == id)!;
    }
}