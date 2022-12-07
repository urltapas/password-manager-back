using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManagerAPI.Data;
using PasswordManagerAPI.Models;
using PasswordManagerAPI.Services;
using PasswordManagerAPI.ViewModels;

namespace PasswordManagerAPI.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    /* register */
    [HttpPost("/api/accounts/register")]
    public async Task<IActionResult> RegisterAsync(
        [FromServices] AppDataContext context,
        [FromBody] UserViewModel model)
    {
        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            PasswordHash = model.Password
        };

        try
        {
            await context.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok("Cadastro concluído com sucesso!");
        }
        catch
        {
            return StatusCode(500, "Ocorreu um erro durante no cadstro da conta!");
        }
    }
    
    /* login */
    [HttpPost("api/accounts/login")]
    public async Task<IActionResult> LoginAsync(
        [FromServices] AppDataContext context,
        [FromServices] TokenService tokenService,
        [FromBody] LoginViewModel model)
    {
        var user = await context.Users
            .AsNoTracking()
            .Include(x => x.Credentials)
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null) return NotFound("Usuário não encontrado!");
        if(user.PasswordHash != model.Password)
            return StatusCode(403, "Usuário ou senha inválidos");

        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(token);
        }
        catch
        {
            return StatusCode(500, "1101 - Falha interna no servidor!");
        }
    }
}
