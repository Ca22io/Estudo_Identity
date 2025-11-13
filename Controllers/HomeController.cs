using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<UsuarioModel> _userManager;

    private readonly SignInManager<UsuarioModel> _signInManager;

    public HomeController(UserManager<UsuarioModel> userManager, SignInManager<UsuarioModel> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
