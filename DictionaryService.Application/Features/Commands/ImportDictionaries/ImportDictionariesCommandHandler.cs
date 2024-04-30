using Common.Exceptions;
using DictionaryService.Domain.Entities;
using DictionaryService.Domain.Enums;
using DictionaryService.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DictionaryService.Application.Features.Commands.ImportDictionaries;

public class ImportDictionariesCommandHandler : IRequestHandler<ImportDictionariesCommand, Unit>
{
    private readonly HttpClient _httpClient;
    private readonly DictionaryDbContext _context;

    public ImportDictionariesCommandHandler(HttpClient httpClient, DictionaryDbContext context)
    {
        _httpClient = httpClient;
        _context = context;
    }

    string url = "https://1c-mockup.kreosoft.space/api/dictionary/";
    private const string username = "student";
    private const string password = "ny6gQnyn4ecbBrP9l1Fz";

    private string _authHeaderValue =
        Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{username}:{password}"));

    public async Task<Unit> Handle(ImportDictionariesCommand request, CancellationToken cancellationToken)
    {
        switch (request.DictionaryType)
        {
            case DictionaryType.EducationLevel:
                return await ImportEducationLevel();
            case DictionaryType.Faculty:
                return await ImportFaculty();
            case DictionaryType.DocumentType:
                return await ImportDocumentType();
            case DictionaryType.Program:
                return await ImportProgram();
            case null:
                await ImportAll();
                break;
        }

        return Unit.Value;
    }

    private async Task<Unit> ImportEducationLevel()
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
            var response = await _httpClient.GetAsync($"{url}education_levels");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var educationLevels = JsonConvert.DeserializeObject<List<EducationLevel>>(result);

                foreach (var educationLevel in educationLevels)
                {
                    var existingEducationLevel = await _context.Set<EducationLevel>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(el => el.Id == educationLevel.Id + 1);

                    if (existingEducationLevel == null)
                    {
                        _context.Set<EducationLevel>().Add(educationLevel);
                    }
                    else
                    {
                        _context.Entry(existingEducationLevel).CurrentValues.SetValues(educationLevel);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new ExternalSystemError(response.StatusCode);
            }

            return Unit.Value;
        }
        catch (Exception ex)
        {
            throw new ExternalSystemException(ex);
        }
    }

    private async Task<Unit> ImportFaculty()
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
            var response = await _httpClient.GetAsync($"{url}faculties");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var faculties = JsonConvert.DeserializeObject<List<Faculty>>(result);

                if (faculties != null)
                    foreach (var faculty in faculties)
                    {
                        var existingFaculty = await _context.Faculties
                            .FirstOrDefaultAsync(f => f.Id == faculty.Id);

                        if (existingFaculty == null)
                        {
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

            return Unit.Value;
        }
        catch (Exception ex)
        {
            throw new ExternalSystemException(ex);
        }
    }

    private async Task<Unit> ImportDocumentType()
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
            var response = await _httpClient.GetAsync($"{url}document_types");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var documentTypes = JsonConvert.DeserializeObject<List<DocumentType>>(result);

                foreach (var documentType in documentTypes)
                {
                    var existingDocumentType = await _context.Set<DocumentType>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(el => el.Id == documentType.Id);

                    var existingEducationLevel = await _context.Set<EducationLevel>()
                        .FirstOrDefaultAsync(el => el.Id == documentType.EducationLevelId);

                    if (existingEducationLevel == null)
                    {
                        throw new BadRequest("You should import education levels first");
                    }
                    
                    if (existingDocumentType == null)
                    {
                        _context.Set<DocumentType>().Add(documentType);
                    }
                    else
                    {
                        _context.Entry(existingDocumentType).CurrentValues.SetValues(documentType);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                throw new ExternalSystemError(response.StatusCode);
            }
            return Unit.Value;
        }
        catch (Exception ex)
        {
            throw new ExternalSystemException(ex);
        }
    }

    private async Task<Unit> ImportProgram()
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", _authHeaderValue);
            var response = await _httpClient.GetAsync($"{url}programs");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var programs = JsonConvert.DeserializeObject<List<Program>>(result);

                foreach (var program in programs)
                {
                    var existingProgram = await _context.Set<Program>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(el => el.Id == program.Id);

                    var existingEducationLevel = await _context.Set<EducationLevel>()
                        .FirstOrDefaultAsync(el => el.Id == program.EducationLevelId);

                    if (existingEducationLevel == null)
                    {
                        throw new BadRequest("You should import education levels first");
                    }
                    
                    var existingFaculty = await _context.Set<Faculty>()
                        .FirstOrDefaultAsync(el => el.Id == program.FacultyId);

                    if (existingFaculty == null)
                    {
                        throw new BadRequest("You should import faculties first");
                    }
                    
                    if (existingProgram == null)
                    {
                        _context.Set<Program>().Add(program);
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
            return Unit.Value;
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