using FindMe.Data;
using FindMe.Models;
using FindMe.Models.Dto.Form;
using FindMe.Models.Dto.Simp;
using FindMe.Models.Results;
using FindMe.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FindMe.Services;

public class SimpService : ISimpService
{
    private readonly ApplicationDbContext _db;

    public SimpService(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<SimpServiceResult> Add(AddSimpDto request)
    {
        if (request is null) 
            return new SimpServiceResult(false,
                "Добавление симпатии. Модель не может быть пустой.");
        
        var isRequestExists = await _db.SimpRequests
            .FirstOrDefaultAsync(sr
                => sr.FromUserId == request.FromUserId &&
                   sr.ToUserId == request.ToUserId);
        if (isRequestExists is not null)
            return new SimpServiceResult(false,
                "Добавление симпатии. Заявка уже была отправлена ранее. Ожидайте ответа.");

        var isAddresseeExists = await _db.Users.FirstOrDefaultAsync(u => u.Id == request.ToUserId);
        if (isAddresseeExists is null)
            return new SimpServiceResult(false,
                "Добавление симпатии. Симпатия отправлена несуществующему пользователю. Попробуйте еще раз.");

        var simpRequest = new SimpRequest()
        {
            ToUserId = request.ToUserId,
            FromUserId = request.FromUserId
        };
        
        await _db.SimpRequests.AddAsync(simpRequest);
        await _db.SaveChangesAsync();

        return new SimpServiceResult(true, "Добавление симпатии. Заявка успешно отправлена.");
    }

    public Task<SimpServiceResult> Match()
    {
        throw new NotImplementedException();
    }

    public async Task<SimpServiceResult> Delete(DeleteSimpDto request)
    {
        if (request is null)
            return new SimpServiceResult(false, "Удаление симпатии. Модель не может быть пустой.");
        
        var simpRequest = await _db.SimpRequests
            .FirstOrDefaultAsync(sr =>
                sr.ToUserId == request.ToUserId &&
                sr.FromUserId == request.FromUserId);
        if (simpRequest is null)
            return new SimpServiceResult(false, "Удалении симпатии. Запрос не был найден.");

        _db.SimpRequests.Remove(simpRequest);
        await _db.SaveChangesAsync();

        return new SimpServiceResult(true, "");
    }

    public Task<SimpServiceResult> Update(UpdateSimpDto requets)
    {
        throw new NotImplementedException();
    }

    public Task<SimpServiceResult> Get()
    {
        throw new NotImplementedException();
    }

    public async Task<List<GetSimpDto>> GetIncoming(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return null;
        
        var requests = await _db.SimpRequests
            .Where(sr => sr.ToUserId == userId)
            .ToListAsync();

        var response = new List<GetSimpDto>();

        foreach (var request in requests)
        {
            var form = await _db.Forms.FirstOrDefaultAsync(f => f.UserId == request.FromUserId);
            response.Add(new GetSimpDto(){
                Form = new GetFormDto()
            {
                Id = form.Id,
                Age = form.Age,
                Description = form.Description,
                Name = form.Name
            },
                ToUserId = request.ToUserId, 
                FromUserId = request.FromUserId});
        }
        
        return response;
    }

    public async Task<List<GetSimpDto>> GetOutgoing(string userId)
    {
        if(string.IsNullOrEmpty(userId))
            return null;
        
        var requests = await _db.SimpRequests
            .Where(sr => sr.FromUserId == userId)
            .ToListAsync();

        var response = new List<GetSimpDto>();

        foreach (var request in requests)
        {
            var form = await _db.Forms.FirstOrDefaultAsync(f => f.UserId == request.ToUserId);
            response.Add(new GetSimpDto(){
                Form = new GetFormDto()
                {
                    Id = form.Id,
                    Age = form.Age,
                    Description = form.Description,
                    Name = form.Name
                },
                ToUserId = request.ToUserId, 
                FromUserId = request.FromUserId});
        }
        
        return response;
    }
}