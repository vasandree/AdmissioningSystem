using System.ComponentModel.DataAnnotations;

namespace Common.Models.Dtos.PagedDtos;

public class ProgramsPagedListDto
{
    [Required]
    public List<ProgramsPagedListDto> Programs { get; set; }
    
    [Required]
    public Pagination Pagination { get; set; }
    
}