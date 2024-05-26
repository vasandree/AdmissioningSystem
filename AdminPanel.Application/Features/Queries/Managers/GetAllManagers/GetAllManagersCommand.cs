using Common.Models.Models.Dtos;
using MediatR;

namespace AdminPanel.Application.Features.Queries.Managers.GetAllManagers;

public record GetAllManagersCommand () : IRequest<List<ManagerDto>>;