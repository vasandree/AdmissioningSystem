using AutoMapper;
using Common.Exceptions;
using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Enums;
using DictionaryService.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Features.Commands.ImportDictionaries;

public class ImportDictionariesCommandHandler : IRequestHandler<ImportDictionariesCommand, Unit>
{
    private readonly HttpClient _httpClient;
    private readonly IEducationLevelRepository _educationLevel;
    private readonly IFacultyRepository _faculty;
    private readonly IDocumentTypeRepository _documentType;
    private readonly IProgramRepository _program;

    private readonly string _url;
    private readonly string _authHeaderValue;

    public ImportDictionariesCommandHandler(HttpClient httpClient, DictionaryDbContext context,
        IConfiguration configuration, IEducationLevelRepository educationLevel, IMapper mapper,
        IFacultyRepository faculty, IDocumentTypeRepository documentType, IProgramRepository program)
    {
        _httpClient = httpClient;
        _educationLevel = educationLevel;
        _faculty = faculty;
        _documentType = documentType;
        _program = program;
        _url = configuration.GetSection("ExternalSystem:BaseURL").Get<string>()!;
        _authHeaderValue =
            Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
                $"{configuration.GetSection("ExternalSystem:Username").Get<string>()!}" +
                $":{configuration.GetSection("ExternalSystem:Password").Get<string>()!}"));
    }


    public async Task<Unit> Handle(ImportDictionariesCommand request, CancellationToken cancellationToken)
    {
        switch (request.DictionaryType)
        {
            case DictionaryType.EducationLevel:
                await ImportEducationLevel();
                break;
            case DictionaryType.Faculty:
                await ImportFaculty();
                break;
            case DictionaryType.DocumentType:
                await ImportDocumentType();
                break;
            case DictionaryType.Program:
                await ImportProgram();
                break;
            case DictionaryType.All:
                await ImportAll();
                break;
        }

        return Unit.Value;
    }

    private async Task ImportEducationLevel()
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
        var response = await _httpClient.GetAsync($"{_url}education_levels");

        if (response.IsSuccessStatusCode)
        {
            var result = response.Content.ReadAsStringAsync().Result;
            var jsonEducationLevels = JsonConvert.DeserializeObject<List<JObject>>(result);


            if (await _educationLevel.CheckIfNotEmpty())
            {
                var toDelete =
                    await _educationLevel.GetEntitiesToDelete(jsonEducationLevels!.Select(e => e.Value<int>("id")));
                await _educationLevel.SoftDeleteEntities(toDelete);

                if (toDelete.Count > 0)
                {
                    var documentTypesToDelete = await _documentType.GetEntitiesToDeleteByEducationLevel(toDelete);
                    if (documentTypesToDelete.Count > 0)
                    {
                        await _documentType.SoftDeleteEntities(documentTypesToDelete!);
                    }


                    var programsToDelete = await _program.GetEntitiesToDeleteByEducationLevel(toDelete);
                    if (programsToDelete.Count > 0)
                    {
                        await _program.SoftDeleteEntities(programsToDelete!);
                    }

                    //todo: send emails
                }
            }

            foreach (var jsonEducationLevel in jsonEducationLevels!)
            {
                if (await _educationLevel.CheckExistenceByExternalId(jsonEducationLevel.Value<int>("id")))
                {
                    var existingEducationLevel =
                        await _educationLevel.GetByExternalId(jsonEducationLevel.Value<int>("id"));

                    if (_educationLevel.CheckIfChanged(existingEducationLevel, jsonEducationLevel))
                    {
                        await _educationLevel.UpdateAsync(existingEducationLevel, jsonEducationLevel);
                    }
                }
                else
                {
                    await _educationLevel.CreateAsync(jsonEducationLevel);
                }
            }
        }
        else
        {
            throw new ExternalSystemError(response.StatusCode);
        }
    }

    private async Task ImportFaculty()
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
        var response = await _httpClient.GetAsync($"{_url}faculties");

        if (response.IsSuccessStatusCode)
        {
            var result = response.Content.ReadAsStringAsync().Result;
            var jsonFaculties = JsonConvert.DeserializeObject<List<JObject>>(result);

            if (await _faculty.CheckIfNotEmpty())
            {
                var toDelete =
                    await _faculty.GetEntitiesToDelete(jsonFaculties!.Select(e =>
                        Guid.Parse(e.Value<string>("id")!)));
                if (toDelete.Count > 0)
                {
                    await _faculty.SoftDeleteEntities(toDelete);
                }


                var programsToDelete = await _program.GetEntitiesToDeleteByFaculty(toDelete);
                if (programsToDelete.Count > 0)
                {
                    await _program.SoftDeleteEntities(programsToDelete!);
                }


                //todo: send emails
            }

            foreach (var jsonFaculty in jsonFaculties!)
            {
                if (await _faculty.CheckExistenceByExternalId(Guid.Parse(jsonFaculty.Value<string>("id")!)))
                {
                    var existingFaculty =
                        await _faculty.GetByExternalId(Guid.Parse(jsonFaculty.Value<string>("id")!));

                    if (_faculty.CheckIfChanged(existingFaculty, jsonFaculty))
                    {
                        await _faculty.UpdateAsync(existingFaculty, jsonFaculty);
                    }
                }
                else
                {
                    await _faculty.CreateAsync(jsonFaculty);
                }
            }
        }
        else
        {
            throw new ExternalSystemError(response.StatusCode);
        }
    }


    private async Task ImportDocumentType()
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
        var response = await _httpClient.GetAsync($"{_url}document_types");

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            var jsonDocumentTypes = JsonConvert.DeserializeObject<List<JObject>>(result);

            if (await _documentType.CheckIfNotEmpty())
            {
                var toDelete =
                    await _documentType.GetEntitiesToDelete(jsonDocumentTypes!.Select(e =>
                        Guid.Parse(e.Value<string>("id")!)));
                if (toDelete.Count > 0)
                {
                    await _documentType.SoftDeleteEntities(toDelete);
                    //todo: delete documents
                }
            }


            foreach (var jsonDocumentType in jsonDocumentTypes!)
            {
                if (await _documentType.CheckExistenceByExternalId(
                        Guid.Parse(jsonDocumentType.Value<string>("id")!)))
                {
                    var existingDocumentType =
                        await _documentType.GetByExternalId(Guid.Parse(jsonDocumentType.Value<string>("id")!));

                    if (await _documentType.CheckIfChanged(existingDocumentType, jsonDocumentType))
                    {
                        await _documentType.UpdateAsync(existingDocumentType, jsonDocumentType);
                    }
                }
                else
                {
                    await _documentType.CreateAsync(jsonDocumentType);
                }
            }
        }
        else
        {
            throw new ExternalSystemError(response.StatusCode);
        }
    }


    private async Task ImportProgram()
    {
        int totalPages;
        int pageNumber = 1;
        int pageSize = 100;
        List<JObject> allPrograms = new List<JObject>();

        do
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
            var response = await _httpClient.GetAsync($"{_url}programs?page={pageNumber}&size={pageSize}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var jsonObject = JsonConvert.DeserializeObject<JObject>(result);
                var jsonPrograms = jsonObject["programs"].ToObject<List<JObject>>();

                allPrograms.AddRange(jsonPrograms);

                totalPages = jsonObject["pagination"]["count"].ToObject<int>();
                pageNumber++;
            }
            else
            {
                throw new ExternalSystemError(response.StatusCode);
            }
        } while (pageNumber <= totalPages);

        if (await _program.CheckIfNotEmpty())
        {
            var toDelete = await _program.GetEntitiesToDelete(allPrograms!.Select(e =>
                Guid.Parse(e.Value<string>("id")!)));
            if (toDelete.Count > 0)
            {
                await _program.SoftDeleteEntities(toDelete);

                //todo: send emails
            }
        }

        foreach (var jsonProgram in allPrograms)
        {
            if (await _program.CheckExistenceByExternalId(
                    Guid.Parse(jsonProgram.Value<string>("id")!)))
            {
                var existingDocumentType =
                    await _program.GetByExternalId(Guid.Parse(jsonProgram.Value<string>("id")!));

                if (await _program.CheckIfChanged(existingDocumentType, jsonProgram))
                {
                    await _program.UpdateAsync(existingDocumentType, jsonProgram);
                }
            }
            else
            {
                await _program.CreateAsync(jsonProgram);
            }
        }
    }


    private async Task ImportAll()
    {
        await ImportEducationLevel();
        await ImportDocumentType();
        await ImportFaculty();
        await ImportProgram();
    }
}