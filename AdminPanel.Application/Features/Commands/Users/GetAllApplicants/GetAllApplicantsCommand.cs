using Common.Models.Models.Dtos.PagedDtos;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.GetAllApplicants;

public record GetAllApplicantsCommand(int Page, int Size) : IRequest<PagedUserDto>;