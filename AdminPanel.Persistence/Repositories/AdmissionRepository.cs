using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Domain.Entities;
using AdminPanel.Infrastructure;
using Common.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Persistence.Repositories;

public class AdmissionRepository : GenericRepository<Admission>, IAdmissionRepository
{
    private readonly AdminPanelDbContext _context;

    public AdmissionRepository(AdminPanelDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> CheckExistence(Guid admissionId)
    {
        return await _context.Admissions.AnyAsync(x => x.Id == admissionId);
    }

    public async Task<Admission> GetById(Guid admissionId)
    {
        return (await _context.Admissions.FirstOrDefaultAsync(x => x.Id == admissionId))!;
    }

    public async Task DeleteManager(Guid admissionId)
    {
        var admission = await GetById(admissionId);
        admission.ManagerId = null;
        await UpdateAsync(admission);
    }
}