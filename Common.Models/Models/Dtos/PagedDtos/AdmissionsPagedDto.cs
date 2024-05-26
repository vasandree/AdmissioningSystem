using System.ComponentModel.DataAnnotations;

namespace Common.Models.Models.Dtos.PagedDtos;

public class AdmissionsPagedDto
{
    [Required] public List<AdmissionDto> Admissions { get; set; }

    [Required] public Pagination Pagination { get; set; }
}