using Microsoft.EntityFrameworkCore;
using UserApi.Application.Contracts.Persistence;
using UserApi.Domain.DbEntities;
using UserApi.Infrastructure;

namespace UserApi.Persistence.Repositories;

public class ApplicantRepository : GenericRepository<ApplicantEntity>, IApplicantRepository
{
    private readonly UserDbContext _context;
    
    protected ApplicantRepository(UserDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<ApplicantEntity?> GetById(Guid id)
    {
        return _context.Applicants.FirstOrDefaultAsync(x => x.UserId == id);
    }
}