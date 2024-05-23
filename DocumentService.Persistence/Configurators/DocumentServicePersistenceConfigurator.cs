using Common.Services.Repository;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Persistence.Configurators;

public static class DocumentServicePersistenceConfigurator
{
    public static void ConfigureDocumentServicePersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddTransient(typeof(IDocumentRepository<>), typeof(DocumentRepository<>));
        builder.Services.AddTransient<IFileRepository,FileRepository >();
    }
}