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

    public void DeleteAdmission(Guid admissionId)
    {
        _bus.PubSub.Publish(new DeleteAdmissionMessage(admissionId));
    }

    public void EditAdmissionPriority(Guid admissionId, int newPriority)
    {
        _bus.PubSub.Publish(new UpdateAdmissionPriorityMessage(admissionId, newPriority));
    }

    public void UpdateStatus(Guid admissionId, AdmissionStatus status)
    {
        _bus.PubSub.Publish(new UpdateAdmissionStatusMessage(admissionId, status));
    }

    public void UpdateManager(Guid admissionId, Guid? managerId = null)
    {
        _bus.PubSub.Publish(new UpdateManagerFromAdmissionMessage(admissionId, managerId));
    }

    public void UpdateRole(Guid userId, string role, Guid? facultyId = null)
    {
        _bus.PubSub.Publish(new UpdateUserRoleMessage(userId, role, facultyId));
    }

    public void SendEmail(string email, string role, Guid? facultyId = null)
    {
        _bus.PubSub.Publish(new ManagerAddedMessage(email, role, facultyId));
    }

    public void UpdateUserInfo(Guid userId, string fullName, string? email = null, string? passwordHash = null,
        Gender? gender = null, string? nationality = null, DateTime? birthDate = null)
    {
        _bus.PubSub.Publish(new UpdateUserInfoMessage(userId, fullName, email, passwordHash, gender, nationality,
            birthDate));
    }

    public void EditUserEducationDocument(Guid userId, string newName)
    {
        _bus.PubSub.Publish(new UpdateEducationDocMessage(userId, newName));
    }

    public void EditUserPassport(Guid userId, string seriesAndNumber, string issuedBy, DateTime dateOfBirth,
        DateTime issueDate)
    {
        _bus.PubSub.Publish(new UpdatePassportMessage(userId, seriesAndNumber, issuedBy, dateOfBirth, issueDate));
    }

    public void DeleteFile(Guid userId, DocumentType documentType)
    {
        _bus.PubSub.Publish(new DeleteFileMessage(userId, documentType));
    }

    public void UploadFile(Guid userId, DocumentType documentType, IFormFile file)
    {
        _bus.PubSub.Publish(new UploadNewFileMessage(userId, documentType, file));
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