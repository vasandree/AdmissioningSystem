using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Domain.Entities;
using AdminPanel.Infrastructure;
using Common.Services.Repository;

namespace AdminPanel.Persistence.Repositories;

public class HeadManagerRepository : GenericRepository<HeadManager>, IHeadManagerRepository
{
    private readonly AdminPanelDbContext _context;

    public HeadManagerRepository(AdminPanelDbContext context) : base(context)
    {
        _context = context;
    }
}