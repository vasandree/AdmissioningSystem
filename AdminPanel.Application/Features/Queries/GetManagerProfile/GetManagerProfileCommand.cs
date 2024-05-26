using Common.Models.Models.Dtos;
using MediatR;

namespace AdminPanel.Application.Features.Queries.GetManagerProfile;

public record GetManagerProfileCommand(Guid ManagerId) : IRequest<ManagerDto>;