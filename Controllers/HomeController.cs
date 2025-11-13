using App.Models;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    

    [HttpGet, Authorize("Adm")]
    public IActionResult Privacy()
    {
        return View();
    }
}
