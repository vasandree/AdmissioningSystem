using System.ComponentModel.DataAnnotations;

namespace Common.Models.Dtos.PagedDtos;

public class FacultiesPagedDto
{
    [Required]
    public List<FacultyDto> Faculties { get; set; }
    
    [Required]
    public Pagination Pagination { get; set; }
}
