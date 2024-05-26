using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Domain.Entities;
using AdminPanel.Infrastructure;
using Common.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Persistence.Repositories;

public class ManagerRepository: GenericRepository<Manager>, IManagerRepository
{
    private readonly AdminPanelDbContext _context;
    
    public ManagerRepository( AdminPanelDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Guid> GetFaculty(Guid mainId)
    {
        return _context.Managers.FirstOrDefaultAsync(x => x.MainId == mainId).Result!.FacultyId;
    }
}