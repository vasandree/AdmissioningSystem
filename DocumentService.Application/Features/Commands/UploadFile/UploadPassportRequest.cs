using DocumentService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace DocumentService.Application.Features.Commands.UploadFile;

public record UploadPassportRequest( IFormFile File, Guid Id) : IRequest<Unit>;