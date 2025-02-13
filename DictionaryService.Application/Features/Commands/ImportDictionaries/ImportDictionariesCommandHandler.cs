using Common.Models.Exceptions;
using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Application.PubSub;
using DictionaryService.Domain.Entities;
using DictionaryService.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Features.Commands.ImportDictionaries;

public class ImportDictionariesCommandHandler : IRequestHandler<ImportDictionariesCommand, Guid>
{
    private readonly HttpClient _httpClient;
    private readonly IEducationLevelRepository _educationLevel;
    private readonly IFacultyRepository _faculty;
    private readonly IDocumentTypeRepository _documentType;
    private readonly IProgramRepository _program;
    private readonly ImportTaskTracker _importTaskTracker;
    private readonly INextEducationLevelRepository _nextEducationLevel;
    private readonly PubSubSender _pubSub;

    private readonly string _url;
    private readonly string _authHeaderValue;

    public ImportDictionariesCommandHandler(HttpClient httpClient,
        IConfiguration configuration, IEducationLevelRepository educationLevel,
        IFacultyRepository faculty, IDocumentTypeRepository documentType, IProgramRepository program,
        ImportTaskTracker importTaskTracker, PubSubSender pubSub, INextEducationLevelRepository nextEducationLevel)
    {
        _httpClient = httpClient;
        _educationLevel = educationLevel;
        _faculty = faculty;
        _documentType = documentType;
        _program = program;
        _importTaskTracker = importTaskTracker;
        _pubSub = pubSub;
        _nextEducationLevel = nextEducationLevel;
        _url = configuration.GetSection("ExternalSystem:BaseURL").Get<string>()!;
        _authHeaderValue =
            Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
                $"{configuration.GetSection("ExternalSystem:Username").Get<string>()!}" +
                $":{configuration.GetSection("ExternalSystem:Password").Get<string>()!}"));
    }


    public async Task<Guid> Handle(ImportDictionariesCommand request, CancellationToken cancellationToken)
    {
        Task importTask;
        switch (request.DictionaryType)
        {
            case DictionaryType.EducationLevel:
                importTask = ImportEducationLevel();
                break;
            case DictionaryType.Faculty:
                importTask = ImportFaculty();
                break;
            case DictionaryType.DocumentType:
                importTask = ImportDocumentType();
                break;
            case DictionaryType.Program:
                importTask = ImportProgram();
                break;
            case DictionaryType.All:
                importTask = ImportAll();
                break;
            default:
                throw new ArgumentException("Invalid dictionary type");
        }

        var taskId = Guid.NewGuid();
        _importTaskTracker.AddTask(taskId, importTask);
        await importTask; //todo: make it async
        return  taskId;
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
                        _pubSub.SoftDeleteDocuments(documentTypesToDelete!);
                    }


                    var programsToDelete = await _program.GetEntitiesToDeleteByEducationLevel(toDelete);
                    if (programsToDelete.Count > 0)
                    {
                        await _program.SoftDeleteEntities(programsToDelete!);
                        _pubSub.SoftDeletePrograms(programsToDelete!);
                    }
                }
            }

            foreach (var jsonEducationLevel in jsonEducationLevels!)
            {
                var newEducationLevel = _educationLevel.Convert(jsonEducationLevel);

                if (await _educationLevel.CheckExistenceByExternalId(newEducationLevel.ExternalId))
                {
                    var existingEducationLevel =
                        await _educationLevel.GetByExternalId(newEducationLevel.ExternalId);

                    if (_educationLevel.CheckIfChanged(existingEducationLevel, newEducationLevel))
                    {
                        await _educationLevel.UpdateAsync(existingEducationLevel, newEducationLevel);
                        
                        _pubSub.UpdateDocumentTypesByEducationLevel(await DocumentTypesToUpdateByEducationLevel(existingEducationLevel));
                        
                        _pubSub.UpdatePrograms(await ProgramsToUpdateDyEducationLevel(existingEducationLevel));
                    }
                }
                else
                {
                    await _educationLevel.CreateAsync(newEducationLevel);
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
                    _pubSub.SoftDeletePrograms(programsToDelete!);
                }
            }

            foreach (var jsonFaculty in jsonFaculties!)
            {
                var newFaculty = _faculty.Convert(jsonFaculty);

                if (await _faculty.CheckExistenceByExternalId(newFaculty.ExternalId))
                {
                    var existingFaculty =
                        await _faculty.GetByExternalId(newFaculty.ExternalId);

                    if (_faculty.CheckIfChanged(existingFaculty, newFaculty))
                    {
                        await _faculty.UpdateAsync(existingFaculty, newFaculty);
                        
                        _pubSub.UpdatePrograms(await ProgramsToUpdateByFaculty(existingFaculty));
                    }
                }
                else
                {
                    await _faculty.CreateAsync(newFaculty);
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
                   _pubSub.SoftDeleteDocuments(toDelete);
                }
            }


            foreach (var jsonDocumentType in jsonDocumentTypes!)
            {
                var newDocumentType = await _documentType.Convert(jsonDocumentType);

                if (await _documentType.CheckExistenceByExternalId(newDocumentType.ExternalId))
                {
                    var existingDocumentType =
                        await _documentType.GetByExternalId(newDocumentType.ExternalId);

                    if (await _documentType.CheckIfChanged(existingDocumentType, newDocumentType,
                            jsonDocumentType["nextEducationLevels"]!.ToObject<List<JObject>>()))
                    {
                        await _documentType.UpdateAsync(existingDocumentType, newDocumentType,
                            jsonDocumentType.Value<List<JObject>>("nextEducationLevels")!);
                        _pubSub.UpdateDocumentType(existingDocumentType);
                    }
                }
                else
                {
                    await _documentType.CreateAsync(newDocumentType,
                        jsonDocumentType["nextEducationLevels"]!.ToObject<List<JObject>>());
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
                _pubSub.SoftDeletePrograms(toDelete!);
            }
        }

        foreach (var jsonProgram in allPrograms)
        {
            var newProgram = await _program.Convert(jsonProgram);

            if (await _program.CheckExistenceByExternalId(newProgram.ExternalId))
            {
                var existingProgram =
                    await _program.GetByExternalId(newProgram.ExternalId);

                if (_program.CheckIfChanged(existingProgram, newProgram))
                {
                    await _program.UpdateAsync(existingProgram, newProgram);
                    _pubSub.UpdateProgram(existingProgram);
                }
            }
            else
            {
                await _program.CreateAsync(newProgram);
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
    
    private async Task<List<Guid>> DocumentTypesToUpdateByEducationLevel(EducationLevel existingEducationLevel)
    {
        var documentTypesToUpdate =
            await _documentType.Find(x => x.EducationLevelId == existingEducationLevel.Id && !x.IsDeleted);

        var documentTypesToUpdateFromNextEducationLevel =
            await _nextEducationLevel.GetDocumentTypesByEducationLevel(existingEducationLevel.Id);

        var documentTypes = documentTypesToUpdate.Concat(documentTypesToUpdateFromNextEducationLevel)
            .GroupBy(dt => dt.Id)
            .Select(group => group.Key)
            .ToList();

        return documentTypes;
    }

    private async Task<IReadOnlyList<Program>> ProgramsToUpdateDyEducationLevel(EducationLevel educationLevel)
    {
        return await _program.Find(x => x.EducationLevel == educationLevel && !x.IsDeleted);
    }

    private async Task<IReadOnlyList<Program>> ProgramsToUpdateByFaculty(Faculty faculty)
    {
        return await _program.Find(x => x.Faculty == faculty && !x.IsDeleted);
    }
}