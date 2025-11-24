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

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Usuarios()
    {
        var usuarios = _userManager.Users.ToList(); 

        var usuariosDto = new List<UsuariosDto>();

        foreach (var user in usuarios)
        {
            var admin = await _userManager.IsInRoleAsync(user, "Admin");

            usuariosDto.Add(new UsuariosDto
            {
                Id = user.Id,
                Nome = user.Nome,
                Email = user.Email,
                Admin = admin
            });
        }

        return View(usuariosDto);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Alterar(int id)
    {
        var usuario = _userManager.FindByIdAsync(id.ToString()).Result;

        var usuarioDto = new UsuariosDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Admin = _userManager.IsInRoleAsync(usuario, "Admin").Result
        };

        return View(usuarioDto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Alterar(UsuariosDto usuario)
    {
        var usuarioExistente = await _userManager.FindByIdAsync(usuario.Id.ToString());
        if (usuarioExistente == null)
        {
            return NotFound();
        }

        usuarioExistente.Nome = usuario.Nome;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.UserName = usuario.Email;

        await _userManager.UpdateAsync(usuarioExistente);

        var admin = await _userManager.IsInRoleAsync(usuarioExistente, "Admin");

        if (usuario.Admin == admin)
        {
            await _userManager.AddToRoleAsync(usuarioExistente, "Admin");
        }
        else if (usuario.Admin == false && admin)
        {
            await _userManager.RemoveFromRoleAsync(usuarioExistente, "Admin");
        }

        return RedirectToAction("Usuarios");
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult Excluir(int id)
    {
        var usuarioExistente = _userManager.FindByIdAsync(id.ToString()).Result;
        if (usuarioExistente == null)
        {
            return NotFound();
        }
        
        var resultado = _userManager.DeleteAsync(usuarioExistente).Result;

        return RedirectToAction("Usuarios");
    }
}