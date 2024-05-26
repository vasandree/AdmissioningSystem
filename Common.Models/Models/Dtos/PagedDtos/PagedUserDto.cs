namespace Common.Models.Models.Dtos.PagedDtos;

public class PagedUserDto
{
    public List<UserDto> Users { get; set; }
    public Pagination Pagination { get; set; }
}