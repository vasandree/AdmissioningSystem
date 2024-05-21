using System.Text.Json.Serialization;

namespace DictionaryService.Application.DTOs;

public class ImportStatusDto
{
    public TaskStatus Status { get; set; }
}