// Em Controllers/AdminController.cs
using App.Dto;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<UsuarioModel> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public AdminController(
        UserManager<UsuarioModel> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Usuarios()
    {
        // Obtém todos os usuários
        var users = _userManager.Users.ToList(); 

        // Mapeia para o ViewModel
        var usuarios = new List<UsuariosDto>();

        foreach (var user in users)
        {
            var is_admin = await _userManager.IsInRoleAsync(user, "Admin");

            usuarios.Add(new UsuariosDto
            {
                Id = user.Id,
                Nome = user.Nome,
                Email = user.Email,
                Admin = is_admin
            });
        }

        Console.WriteLine(usuarios.FindAll(u => u.Admin));

        return View(usuarios);
    }

}