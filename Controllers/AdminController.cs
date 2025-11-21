// Em Controllers/AdminController.cs
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

}