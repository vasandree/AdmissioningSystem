namespace DocumentService.Domain.Entities;

public class Passport : Document
{
    public string SeriesAndNumber { get; set; }
    public string PlaceOfBirth { get; set; }
    public string IssuedBy { get; set; }
    public DateTime IssueDate { get; set; }
}