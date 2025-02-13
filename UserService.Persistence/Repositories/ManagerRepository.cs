using Common.Services.Repository;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Contracts.Persistence;
using UserApi.Domain.DbEntities;
using UserService.Infrastructure;

namespace UserService.Persistence.Repositories;

public class ManagerRepository : GenericRepository<ManagerEntity>, IManagerRepository
{
    private readonly UserDbContext _context;
    
    public ManagerRepository(UserDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<ManagerEntity?> GetById(Guid id)
    {
        return _context.Managers.FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task<IReadOnlyList<ManagerEntity>> GetManagers()
    {
        return await _context.Managers.Where(x => x.Faculty != null).AsNoTracking().ToListAsync();
    }


    public async Task<IReadOnlyList<ManagerEntity>> GetHeadManagers()
    {
        return await _context.Managers.Where(x => x.Faculty == null).AsNoTracking().ToListAsync();
    }

    public async Task<IReadOnlyList<ManagerEntity>> GetManagersByFaculty(Guid id)
    {
        return await _context.Managers.Where(x => x.Faculty == id).AsNoTracking().ToListAsync();
    }
}