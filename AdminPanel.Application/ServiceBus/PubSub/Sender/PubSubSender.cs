using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Models.Enums;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.AspNetCore.Http;

namespace AdminPanel.Application.ServiceBus.PubSub.Sender;

public class PubSubSender
{
    private readonly IBus _bus;
    private readonly RpcRequestSender _rpc;

    public PubSubSender(IBus bus, RpcRequestSender rpc)
    {
        _bus = bus;
        _rpc = rpc;
    }

    public async Task DeleteAdmission(Guid admissionId)
    {
        await _bus.PubSub.PublishAsync(new DeleteAdmissionMessage(admissionId));
    }

    public async Task EditAdmissionPriority(Guid admissionId, int newPriority)
    {
       await _bus.PubSub.PublishAsync(new UpdateAdmissionPriorityMessage(admissionId, newPriority));
    }

    public async Task UpdateStatus(Guid admissionId, AdmissionStatus status)
    {
        await _bus.PubSub.PublishAsync(new UpdateAdmissionStatusMessage(admissionId, status));
    }

    public async Task UpdateManager(Guid admissionId, Guid? managerId = null)
    {
        await _bus.PubSub.PublishAsync(new UpdateManagerFromAdmissionMessage(admissionId, managerId));
    }

    public async Task UpdateRole(Guid userId, string role, Guid? facultyId = null)
    {
        await _bus.PubSub.PublishAsync(new UpdateUserRoleMessage(userId, role, facultyId));
    }

    public async Task SendEmail(string email, string role, Guid? facultyId = null)
    {
        await _bus.PubSub.PublishAsync(new ManagerAddedMessage(email, role, facultyId));
    }

    public async Task UpdateEmailAndFullName(Guid userId, string fullName, string email)
    {
        await _bus.PubSub.PublishAsync(new UpdateEmailAndFullNameMessage(userId, fullName, email));
    }
    
    
    public async Task UpdateUserInfo(Guid userId, string fullName,
        Gender? gender, string? nationality, DateTime? birthDate)
    {
       await _bus.PubSub.PublishAsync(new UpdateUserInfoMessage(userId, fullName,  gender, nationality,
            birthDate));
    }

    public async Task EditUserEducationDocument(Guid userId, string newName)
    {
        await _bus.PubSub.PublishAsync(new UpdateEducationDocMessage(userId, newName));
    }

    public async Task EditUserPassport(Guid userId, string seriesAndNumber, string issuedBy, DateTime dateOfBirth,
        DateTime issueDate)
    {
        await _bus.PubSub.PublishAsync(new UpdatePassportMessage(userId, seriesAndNumber, issuedBy, dateOfBirth, issueDate));
    }

    public async Task DeleteFile(Guid userId, DocumentType documentType)
    {
        await _bus.PubSub.PublishAsync(new DeleteFileMessage(userId, documentType));
    }

    public async Task UploadFile(Guid userId, DocumentType documentType, IFormFile file)
    {
        await _bus.PubSub.PublishAsync(new UploadNewFileMessage(userId, documentType, file));
    }


    public void SendEmailToManager(string email, Guid admissionId)
    {
        _bus.PubSub.Publish(new EmailToManagerMessage(email, admissionId));
    }

    public void SendEmailToApplicant(Guid admissionId)
    {
        _bus.PubSub.Publish(new GetApplicantMessage(admissionId));
    }
}