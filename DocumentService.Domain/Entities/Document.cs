using DocumentService.Domain.Enums;

namespace DocumentService.Domain.Entities;

public abstract class Document
{
    public DocumentType DocumentType { get; set; }
}