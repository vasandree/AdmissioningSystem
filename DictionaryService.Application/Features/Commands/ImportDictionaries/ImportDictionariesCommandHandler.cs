using Common.Exceptions;
using DictionaryService.Domain.Entities;
using DictionaryService.Domain.Enums;
using DictionaryService.Infrastructure;
using DictionaryService.Persistence.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DictionaryService.Application.Features.Commands.ImportDictionaries;

public class ImportDictionariesCommandHandler : IRequestHandler<ImportDictionariesCommand, Unit>
{
    private readonly HttpClient _httpClient;
    private readonly DictionaryDbContext _context;
    private readonly ConvertHelper _convertHelper;
    private readonly DeletionCheckHelper _deletionCheckHelper;
    private readonly UpdateHelper _updateHelper;

    public ImportDictionariesCommandHandler(HttpClient httpClient, DictionaryDbContext context, ConvertHelper helper, DeletionCheckHelper deletionCheckHelper, UpdateHelper updateHelper)
    {
        _httpClient = httpClient;
        _context = context;
        _convertHelper = helper;
        _deletionCheckHelper = deletionCheckHelper;
        _updateHelper = updateHelper;
    }

    private const string Url = "https://1c-mockup.kreosoft.space/api/dictionary/";
    private const string Username = "student";
    private const string Password = "ny6gQnyn4ecbBrP9l1Fz";

    private string _authHeaderValue =
        Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{Username}:{Password}"));

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
            case null:
                await ImportAll();
                break;
        }

        return Unit.Value;
    }

    private async Task ImportEducationLevel()
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
            var response = await _httpClient.GetAsync($"{Url}education_levels");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var jsonEducationLevels = JsonConvert.DeserializeObject<List<JObject>>(result);

                foreach (var jsonEducationLevel in jsonEducationLevels!)
                {
                    var educationLevel = await _convertHelper.ConvertToEducationKLevel(jsonEducationLevel);

                    var existingEducationLevel = await _context.Set<EducationLevel>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(el => el.ExternalId == educationLevel.ExternalId);

                    if (existingEducationLevel == null)
                    {
                        _context.Set<EducationLevel>().Add(educationLevel);
                    }
                    else
                    {
                        _updateHelper.UpdateEducationLevel(educationLevel, existingEducationLevel, _context);
                    }

                    await _context.SaveChangesAsync();
                }
                
                _deletionCheckHelper.EducationLevelDeletionCheck(jsonEducationLevels, _context);
                
            }
            else
            {
                throw new ExternalSystemError(response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            throw new ExternalSystemException(ex);
        }
    }

    private async Task ImportFaculty()
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
            var response = await _httpClient.GetAsync($"{Url}faculties");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var jsonFaculties = JsonConvert.DeserializeObject<List<JObject>>(result);


                foreach (var jsonFaculty in jsonFaculties!)
                {
                    var faculty = await _convertHelper.ConvertToFaculty(jsonFaculty);

                    var existingFaculty = await _context.Faculties
                        .FirstOrDefaultAsync(f => f.ExternalId == faculty.ExternalId);

                    if (existingFaculty == null)
                    {
                        faculty.Id = new Guid();
                        _context.Faculties.Add(faculty);
                    }
                    else
                    {
                        _context.Entry(faculty).State = EntityState.Modified;
                    }

                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new ExternalSystemError(response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            throw new ExternalSystemException(ex);
        }
    }

    private async Task ImportDocumentType()
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
            var response = await _httpClient.GetAsync($"{Url}document_types");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var jsonDocumentTypes = JsonConvert.DeserializeObject<List<JObject>>(result);

                foreach (var jsonDocumentType in jsonDocumentTypes!)
                {
                    var documentType = await _convertHelper.ConvertToDocumentType(jsonDocumentType, _context);

                    var existingDocumentType = await _context.DocumentTypes
                        .FirstOrDefaultAsync(dt => dt.ExternalId == documentType.ExternalId);

                    if (existingDocumentType == null)
                    {
                        _context.DocumentTypes.Add(documentType);
                    }
                    else
                    {
                        _context.Entry(existingDocumentType).CurrentValues.SetValues(documentType);
                    }
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ExternalSystemError(response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            throw new ExternalSystemException(ex);
        }
    }


    private async Task ImportProgram()
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
            var response = await _httpClient.GetAsync($"{Url}programs?page=1&size=500");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var jsonObject = JsonConvert.DeserializeObject<JObject>(result);
                var jsonPrograms = jsonObject!["programs"]!.ToObject<List<JObject>>();

                foreach (var jsonProgram in jsonPrograms!)
                {
                    var program = await _convertHelper.ConvertToProgram(jsonProgram, _context);

                    var existingProgram =
                        await _context.Programs.FirstOrDefaultAsync(p => p.ExternalId == program.ExternalId);

                    if (existingProgram == null)
                    {
                        _context.Programs.Add(program);
                    }
                    else
                    {
                        _context.Entry(existingProgram).CurrentValues.SetValues(program);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new ExternalSystemError(response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            throw new ExternalSystemException(ex);
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