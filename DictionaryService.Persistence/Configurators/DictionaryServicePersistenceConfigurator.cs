using Common.Repository;
using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Persistence.Helpers.Converters;
using DictionaryService.Persistence.Helpers.Update;
using DictionaryService.Persistence.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryService.Persistence.Configurators;

public static class DictionaryServicePersistenceConfigurator
{
    public static void ConfigureDictionaryServicePersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddTransient(typeof(IDictionaryRepository<>), typeof(DictionaryRepository<>));
        builder.Services.AddTransient<IEducationLevelRepository, EducationLevelRepository>();
        builder.Services.AddTransient<IFacultyRepository, FacultyRepository>();
        builder.Services.AddTransient<IDocumentTypeRepository, DocumentTypeRepository>();
        builder.Services.AddTransient<IProgramRepository, ProgramRepository>();
        builder.Services.AddTransient<INextEducationLevelRepository, NextEducationLevelRepository>();
        
        builder.Services.AddScoped<DocumentTypeConverter>(); 
        builder.Services.AddScoped<EducationLevelConverter>(); 
        builder.Services.AddScoped<FacultyConverter>(); 
        builder.Services.AddScoped<ProgramConverter>(); 
        
        builder.Services.AddScoped<DocumentTypeUpdate>(); 
        builder.Services.AddScoped<EducationLevelUpdate>();
        builder.Services.AddScoped<FacultyUpdate>(); 
        builder.Services.AddScoped<ProgramUpdate>();

    }
}