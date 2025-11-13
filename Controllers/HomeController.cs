using App.Dto;
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

    [HttpGet]
    public IActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Cadastrar(UsuarioCadastroDto usuario)
    {
        if (ModelState.IsValid)
        {
            var usuarioModel = new UsuarioModel { UserName = usuario.Email, Email = usuario.Email, Nome = usuario.Nome, Cpf = usuario.Cpf };

            var resultado = await _userManager.CreateAsync(usuarioModel, usuario.Password);

            if (resultado.Succeeded)
            {
                await _signInManager.SignInAsync(usuarioModel, isPersistent: false, null);

                return RedirectToAction("Index", "Home");
            }

            foreach (var erro in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, erro.Description);
            }
        }

        return View(usuario);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password, bool rememberMe)
    {
        if (ModelState.IsValid)
        {
            var resultado = await _signInManager.PasswordSignInAsync(email, password, isPersistent: rememberMe, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Login inválido.");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AcessoNegado()
    {
        return View();
    }

    [HttpGet, Authorize("Adm")]
    public IActionResult Privado()
    {
        return View();
    }
}
