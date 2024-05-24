using FindMe.Data;
using FindMe.Models;
using FindMe.Models.Dto.Form;
using FindMe.Models.Results;
using FindMe.Services.Caching.Interfaces;
using FindMe.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FindMe.Services;

public class FormService : IFormService
{
    private readonly ApplicationDbContext _db;
    private readonly IDistributedCacheService _cache;
    private readonly ILogger<FormService> _logger;

    public FormService(ApplicationDbContext db, IDistributedCacheService cache, ILogger<FormService> logger)
    {
        _db = db;
        _cache = cache;
        _logger = logger;
    }

    public async Task<FormServiceResult> Create(CreateFormDto? request)
    {
        if (request is null) return new FormServiceResult(false, "Создание формы. Модель формы не может быть пустой.");
        if (await _db.Forms
                .FirstOrDefaultAsync(f => f.UserId == request.UserId) is not null)
        {
            return new FormServiceResult(false, "У вас уже есть заполненная анкета!");
        }

        var form = new Form()
        {
            Age = request.Age,
            Name = request.Name,
            Description = request.Description,
            UserId = request.UserId
        };
        
        await _db.Forms.AddAsync(form);
        await _db.SaveChangesAsync();

        return new FormServiceResult(true, $"Анкета была успешно добавлена");
    }

    public async Task<GetFormDto?> Get(string formId)
    {
        if (string.IsNullOrEmpty(formId)) return null;

        var cache = await _cache.GetData<Form>($"form-{formId}");
        if (cache is not null)
        {
            return new GetFormDto()
            {
                Id = cache.Id,
                Age = cache.Age,
                Description = cache.Description,
                Name = cache.Name
            };
        }
        
        var form = await _db.Forms.FirstOrDefaultAsync(f => f.Id == formId);
        if (form == null)
        {
            return null;
        }

        await _cache.SetData($"form-{formId}", form, TimeSpan.FromMinutes(3));
        
        return new GetFormDto()
        {
            Id = form.Id,
            Name = form.Name,
            Description = form.Description,
            Age = form.Age
        };
    }

    public async Task<GetFormDto?> GetByInterests(string userId)
    {
        if (string.IsNullOrEmpty(userId)) return null;
        
        var userForm = await _db.Forms.FirstOrDefaultAsync(f => f.UserId == userId);
        if (userForm is null) return null;

        var userInterestIds = await _db.UserInterests
            .Where(ui => ui.FormId == userForm.Id)
            .Select(ui => ui.InterestId)
            .ToListAsync();

        var viewedForms = await _db.ViewedForms
            .Where(vf => vf.FormId == userForm.Id)
            .Select(vf => vf.ViewedFormId)
            .ToListAsync();
        
        var responseForm = await _db.Forms
            .Include(f => f.UserInterests)
            .Where(f => f.UserInterests.Any(ui =>
                userInterestIds.Contains(ui.InterestId))
            && !viewedForms.Contains(f.Id)
            && f.UserId != userId)
            .FirstOrDefaultAsync();
            
        return new GetFormDto()
        {
            Id = responseForm.Id,
            Age = responseForm.Age,
            Name = responseForm.Name,
            Description = responseForm.Description
        };
    }
    public async Task<GetFormDto?> GetFormForCurrentUser(string userId)
    {
        if (string.IsNullOrEmpty(userId)) return null;
        
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var form = await _db.Forms.FirstOrDefaultAsync(f => f.UserId == user.Id);
        
        if (form is null) return null;

        return new GetFormDto()
        {
            Id = form.Id,
            Name = form.Name,
            Description = form.Description,
            Age = form.Age
        };
    }

    public async Task<FormServiceResult> Update(UpdateFormDto? request)
    {
        if (request is null) return new FormServiceResult(false, "Обновление данных формы. Модель формы не может быть пустой.");
        var form = new Form()
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            Age = request.Age
        };
        _db.Forms.Update(form);
        await _db.SaveChangesAsync();

        return new FormServiceResult(true, $"Анкета была успешно обновлена");
    }

    public async Task<FormServiceResult> DeleteById(string formId)
    {
        if (string.IsNullOrEmpty(formId)) return new FormServiceResult(false, "Удаление анкеты по Id. Задан пустой идентификатор формы");

        await _cache.RemoveData($"form-{formId}"); 
        
        var form = await _db.Forms.FirstAsync(f => f.Id == formId);
        _db.Forms.Remove(form);
        
        await _db.SaveChangesAsync();
        return new FormServiceResult(true, $"Анкета была успешно удалена");
    }

    public async Task<FormServiceResult> Delete(DeleteFormDto? request)
    {
        if (request is null) return new FormServiceResult(false, "Удаление анкеты. Модель не может быть пустой.");

        await _cache.RemoveData($"form-{request.Id}");
        
        var form = new Form()
        {
            Age = request.Age,
            Id = request.Id,
            Description = request.Description,
            Name = request.Name
        };
        
        _db.Forms.Remove(form);
        await _db.SaveChangesAsync();

        return new FormServiceResult(true, "Форма успешно удалена.");
    }
}