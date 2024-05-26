using AdminPanel.Application.Features.Queries.Admissions.GetAllAdmissions;
using Common.Models.Models;
using Common.Models.Models.Enums;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminPanel.Application.ServiceBus.Rpc;

public class RpcRequestSender
{
    private readonly IBus _bus;

    public RpcRequestSender(IBus bus)
    {
        _bus = bus;
    }

    public async Task<bool> CheckStatusClosed(Guid admissionId)
    {
        var status =
            await _bus.Rpc.RequestAsync<CheckAdmissionStatusRequest, CheckAdmissionStatusResponse>(
                new CheckAdmissionStatusRequest(admissionId));

        return status.Closed;
    }

    public async Task<bool> CheckPriorityAvailable(Guid admissionId, int newPriority)
    {
        var response = await _bus.Rpc.RequestAsync<CheckPriorityAvailableRequest, CheckPriorityAvailableResponse>(
            new CheckPriorityAvailableRequest(admissionId, newPriority));

        return response.Closed;
    }

    public async Task<bool> CheckFaculty(Guid admissionId, Guid facultyId)
    {
        var response = await _bus.Rpc.RequestAsync<CheckManagerFacultyRequest, CheckManagerFacultyResponse>(
            new CheckManagerFacultyRequest(admissionId, facultyId));

        return response.Valid;
    }

    public async Task<GetUserResponse> GetUser(Guid userId)
    {
        var response = await _bus.Rpc.RequestAsync<GetUserRequest, GetUserResponse>(new GetUserRequest(userId));

        return response;
    }

    public async Task<bool> CheckFaculty(Guid facultyId)
    {
        var response =
            await _bus.Rpc.RequestAsync<CheckFacultyRequest, CheckFacultyResponse>(new CheckFacultyRequest(facultyId));

        return response.Exists;
    }

    public async Task<bool> NotMyAdmission(Guid admissionId, Guid managerId)
    {
        var response =
            await _bus.Rpc.RequestAsync<CheckNotMyAdmissionRequest, CheckNotMyResponse>(
                new CheckNotMyAdmissionRequest(admissionId, managerId));

        return response.NotMy;
    }

    public async Task<bool> ChecFacultyByUserId(Guid userId, Guid facultyId)
    {
        var response =
            await _bus.Rpc.RequestAsync<CheckFacultyByUserIdRequest, CheckFacultyByUserIdResponse>(
                new CheckFacultyByUserIdRequest(userId, facultyId));

        return response.Available;
    }

    public async Task<bool> CheckDocumentExistence(Guid userId, DocumentType documentType, bool checkFiles = false)
    {
        var response =
            await _bus.Rpc.RequestAsync<CheckDocumentExistenceRequest, CheckDocumentExistenceResponse>(
                new CheckDocumentExistenceRequest(userId, documentType, checkFiles));

        return response.Exists;
    }

    public async Task<GetAllAdmissionsResponse> GetAdmissions(GetAllAdmissionsQuery request)
    {
        var response =
            await _bus.Rpc.RequestAsync<GetAllAdmissionsRequest, GetAllAdmissionsResponse>(
                new GetAllAdmissionsRequest(request.Faculties, request.Program, request.Status, request.My,
                    request.ManagerId, request.Name, request.Size, request.Page, request.NoManager));

        return response;
    }

    public async Task<bool> CheckManagersApplicant(Guid managerId, Guid userId)
    {
        var response =
            await _bus.Rpc.RequestAsync<CheckManagersApplicantRequest, CheckNotMyResponse>(
                new CheckManagersApplicantRequest(managerId, userId));

        return response.NotMy;
    }

    public async Task<List<AdmissionDto>> GetApplicanrAdmissions(Guid applicantId)
    {
        var response =
            await _bus.Rpc.RequestAsync<GetAllApplicantAdmissionsRequest, GetAllApplicantAdmissionsResponse>(
                new GetAllApplicantAdmissionsRequest(applicantId));

        return response.Admissions;
    }

    public async Task<object?> GetEducationDocument(Guid applicantId)
    {
        var response =
            await _bus.Rpc.RequestAsync<EducationDocumentRequest, EducationDocumentInfoResponse>(
                new EducationDocumentRequest(applicantId));

        return response.EducationDocument;
    }

    public async Task<object?> GetFile(Guid applicantId, DocumentType documentType)
    {
        var response =
            await _bus.Rpc.RequestAsync<GetFileRequest, GetFileResponse>(new GetFileRequest(applicantId, documentType));

        return response.File;
    }

    public async Task<object> GetUserInfo(Guid applicantId)
    {
        var response =
            await _bus.Rpc.RequestAsync<GetUserInfoRequest, GetUserInfoResponse>(new GetUserInfoRequest(applicantId));

        return response.UserInfo;
    }

    public async Task<object> GetPassport(Guid applicantId)
    {
        var response =
            await _bus.Rpc.RequestAsync<GetPassportRequest, GetPassportResponse>(new GetPassportRequest(applicantId));
        
        return response;
    }
}