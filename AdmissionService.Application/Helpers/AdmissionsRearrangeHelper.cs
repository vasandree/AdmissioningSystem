using AdmissionService.Application.Contracts.Persistence;
using AdmissionService.Domain.Entities;

namespace AdmissionService.Application.Helpers;

public class AdmissionsRearrangeHelper
{
    private readonly IAdmissionRepository _admission;

    public AdmissionsRearrangeHelper(IAdmissionRepository admission)
    {
        _admission = admission;
    }

    public async Task RearrangeAdmissionsByAddingNewOne(Guid userId, int priority)
    {
        var admissions = await _admission.GetApplicantsAdmissions(userId);

        var admissionsToUpdate = admissions.Where(a => a.Priority >= priority).ToList();

        await DecreasePriorities(admissionsToUpdate);
    }

    public async Task RearrangeAdmissions(Guid userId, int oldPriority, int newPriority)
    {
        var admissions = await _admission.GetApplicantsAdmissions(userId);

        if (newPriority < oldPriority)
        {
            var admissionsToUpdate =
                admissions.Where(a => a.Priority >= newPriority && a.Priority < oldPriority).ToList();

            await DecreasePriorities(admissionsToUpdate);
        }
        else if (newPriority > oldPriority)
        {
            var admissionsToUpdate =
                admissions.Where(a => a.Priority < newPriority && a.Priority <= oldPriority).ToList();

            await IncreasePriorities(admissionsToUpdate);
        }
    }

    public async Task RearrangeAdmissionsByDeletion(Guid userId, int priority)
    {
        var admissions = await _admission.GetApplicantsAdmissions(userId);

        var admissionsToUpdate = admissions.Where(a => a.Priority > priority).ToList();

        await IncreasePriorities(admissionsToUpdate);
    }
    
    private async Task DecreasePriorities(List<Admission> admissions)
    {
        foreach (var admission in admissions)
        {
            admission.Priority++;
            await _admission.UpdateAsync(admission);
        }
    }

    private async Task IncreasePriorities(List<Admission> admissions)
    {
        foreach (var admission in admissions)
        {
            admission.Priority--;
            await _admission.UpdateAsync(admission);
        }
    }
}