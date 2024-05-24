using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FindMe.Models.Identity.Dto;
using FindMe.Models.Identity.Results;
using FindMe.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FindMe.Services;

public class AuthentificationService : IAuthentificationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _config;
    public AuthentificationService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _config = config;
    }
    public async Task<RegisterResult> Register(RegisterDto? userModel)
    {
        if (userModel == null) return new RegisterResult(false, "Модель не может быть пустой!");
        
        var user = await _userManager.FindByEmailAsync(userModel.Email);
        if (user is not null) return new RegisterResult(false, "Email уже занят");

        var newUser = new IdentityUser()
        {
            Email = userModel.Email,
            UserName = userModel.Email
        };
        
        var createUser = await _userManager.CreateAsync(newUser, userModel.Password);
        if (!createUser.Succeeded) return new RegisterResult(false, "Непредвиденная ошибка. Попробуйте ещё разже");
        
        //Если пользователь - первый зарегестрировавшийся, то он - Администратор
        //иначе - просто юзер
        var checkAdmin = await _roleManager.FindByNameAsync("Admin");
        if (checkAdmin is null)
        {
            await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            await _userManager.AddToRoleAsync(newUser, "Admin");
            return new RegisterResult(true, "Аккаунт создан");
        }
        else
        {
            var checkUser = await _roleManager.FindByNameAsync("User");
            if (checkUser is null)
                await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });

            await _userManager.AddToRoleAsync(newUser, "User");
            return new RegisterResult(true, "Аккаунт создан");
        }
    }

    public async Task<LoginResult> Login(LoginDto? model)
    {
        if (model is null) return new LoginResult(false, "","Модель не может быть пустой");

        var getUser = await _userManager.FindByEmailAsync(model.Email);
        if (getUser is null) return new LoginResult(false, "", "Пользователь с указанным Email не был найден");

        var checkPassword = await _userManager.CheckPasswordAsync(getUser, model.Password);
        if (checkPassword == false) return new LoginResult(false, "", "Неправильный пароль");

        var userRole = await _userManager.GetRolesAsync(getUser);
        var userInfo = new CreateTokenDto()
        {
            Email = getUser.Email,
            Id = getUser.Id,
            Role = userRole.First()
        };
        
        string token = GenerateJwtToken(userInfo);
        return new LoginResult(true, token, "Успешный вход!");
    }

    private string GenerateJwtToken(CreateTokenDto userInfo)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]
                                                                          ?? throw new KeyNotFoundException("Не удалось найти поле Jwt:Key в appsettings.json")));
        var creditionals = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
            new Claim(ClaimTypes.Name, userInfo.Email),
            new Claim(ClaimTypes.NameIdentifier, userInfo.Id),
            new Claim(ClaimTypes.Email, userInfo.Email),
            new Claim(ClaimTypes.Role, userInfo.Role),
        };
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creditionals);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}