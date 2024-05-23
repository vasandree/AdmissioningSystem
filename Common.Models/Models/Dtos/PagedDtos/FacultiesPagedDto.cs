using System.ComponentModel.DataAnnotations;

namespace Common.Models.Models.Dtos.PagedDtos;

public class FacultiesPagedDto
{
    [Required] public List<FacultyDto> Faculties { get; set; }

    [Required] public Pagination Pagination { get; set; }
}