using System.ComponentModel.DataAnnotations;

namespace Common.Models.Models.Dtos.PagedDtos;

public class ProgramsPagedListDto
{
    [Required]
    public List<ProgramDto> Programs { get; set; }
    
    [Required]
    public Pagination Pagination { get; set; }
    
}
