using Common.Services.Repository;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Contracts.Persistence;
using UserApi.Domain.DbEntities;
using UserService.Infrastructure;

namespace UserService.Persistence.Repositories;

public class ApplicantRepository : GenericRepository<ApplicantEntity>, IApplicantRepository
{
    private readonly UserDbContext _context;
    
    public ApplicantRepository(UserDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<ApplicantEntity?> GetById(Guid id)
    {
        return _context.Applicants.FirstOrDefaultAsync(x => x.UserId == id);
    }
}