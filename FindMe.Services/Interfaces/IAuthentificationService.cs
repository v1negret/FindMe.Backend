using FindMe.Models.Identity.Dto;
using FindMe.Models.Identity.Results;

namespace FindMe.Services.Interfaces;

public interface IAuthentificationService
{
    public Task<RegisterResult> Register(RegisterDto user);
    public Task<LoginResult> Login(LoginDto user);
}