using FindMe.Data;
using FindMe.Models;
using FindMe.Models.Dto.Interest;
using FindMe.Models.Results;
using FindMe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FindMe.Services;

public class InterestsService : IInterestsService
{
    private readonly ApplicationDbContext _db;

    public InterestsService(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<GetInterestDto> Get(int interestId)
    {
        var interest = await _db.Interests.FirstOrDefaultAsync(i => i.Id == interestId);
        if (interest is null) return null;

        return new GetInterestDto() { Id = interest.Id, Name = interest.Name };
    }

    public async Task<GetAllInterestsDto> GetAll()
    {
        var interestList = await _db.Interests.ToListAsync();
        return new GetAllInterestsDto()
        {
            Result = true,
            Interests = interestList
        };
    }

    public async Task<GetAllInterestsForUserDto> GetAllForUser(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return new GetAllInterestsForUserDto(){ Result = false, Interests = null };
        var form = await _db.Forms.FirstOrDefaultAsync(u => u.UserId == userId);
        var interests = await _db.UserInterests
            .Include(ui => ui.Interest)
            .Where(ui => ui.FormId == form.Id)
            .Select(ui => new Interest(){Id= ui.InterestId, Name = ui.Interest.Name})
            .ToListAsync();
        if (interests.Count < 1)
            return new GetAllInterestsForUserDto() { Result = false, Interests = null };

        return new GetAllInterestsForUserDto() { Result = true, Interests = interests };
    }

    public async Task<InterestsServiceResult> Create(CreateInterestDto? request)
    {
        if (request is null) return new InterestsServiceResult(false, "Добавление нового интереса. ОМодель не может быть пустой");
        var interest = new Interest()
        {
            Name = request.Name
        };
        await _db.Interests.AddAsync(interest);
        await _db.SaveChangesAsync();
        return new InterestsServiceResult(true, "Интерес успешно добавлен");
    }

    public async Task<InterestsServiceResult> Delete(DeleteInterestDto? request)
    {
        if (request is null) return new InterestsServiceResult(false, "Удаление интереса. Модель не может быть пустой");
        var interest = new Interest()
        {
            Id = request.Id,
            Name = request.Name
        };
        _db.Interests.Remove(interest);
        await _db.SaveChangesAsync();

        return new InterestsServiceResult(true, "Интерес успешно удален");
    }

    public async Task<InterestsServiceResult> Update(UpdateInterestDto? request)
    {
        if (request is null) return new InterestsServiceResult(false, "Обновление интереса. Модель не можзет быть пустой");
        var interest = new Interest()
        {
            Id = request.Id,
            Name = request.Name
        };
        _db.Interests.Update(interest);
        await _db.SaveChangesAsync();
        
        return new InterestsServiceResult(true, "Интерес успешно обновлен");
    }

    public async Task<InterestsServiceResult> AddToUser(AddInterestsToUserDto request)
    {
        if (request is null)
            return new InterestsServiceResult(false, "Добавление интересов пользователю. Модель не может быть пустой");
        if (request.Interests.Count < 1)
            return new InterestsServiceResult(false, "Добавление интересов пользователю. Необходимо выбрать хотя бы один интерес");
        
        foreach (var interestId in request.Interests)
        {
            var interest = await _db.Interests.FirstOrDefaultAsync(i => i.Id == interestId);
            if (interest is null)
                return new InterestsServiceResult(false,"Добавление интересов пользователю. Данный интерес не был найден");
            var userInterest = new UserInterest(){FormId = request.FormId, InterestId = interestId};
            
            await _db.UserInterests.AddAsync(userInterest);
        }

        await _db.SaveChangesAsync();
        return new InterestsServiceResult(true, "Добавление интересов пользователю прошло успешно");
    }
}