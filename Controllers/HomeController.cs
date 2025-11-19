using System.Diagnostics;
using System.Text;
using App.Dto;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace App.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<UsuarioModel> _userManager;
    private readonly SignInManager<UsuarioModel> _signInManager;
    private readonly IEmailService _emailService;

    public HomeController(UserManager<UsuarioModel> userManager, SignInManager<UsuarioModel> signInManager, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
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
                await EnviarLinkDeConfirmacao(usuarioModel);

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
    public async Task<IActionResult> Login(UsuarioLoginDto usuario)
    {
        if (ModelState.IsValid)
        {
            var resultado = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Password, isPersistent: usuario.RememberMe, lockoutOnFailure: false);
            if (resultado.Succeeded)
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByEmailAsync(usuario.Email);

            if (user != null)
            {
                // Verifica se a senha estava correta, mas o e-mail não foi confirmado
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    // MENSAGEM PARA O USUÁRIO
                    ModelState.AddModelError(string.Empty, "Seu e-mail ainda não foi confirmado. Uma nova mensagem de confirmação foi enviada.");
                    
                    // Lógica para REENVIAR o e-mail
                    await EnviarLinkDeConfirmacao(user, "Reenvio de Confirmação de E-mail");
                    
                    return View();
                }
                
                // Caso o erro seja devido a bloqueio por tentativas (lockout)
                if (resultado.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Sua conta está bloqueada devido a várias tentativas de login fracassadas. Tente novamente mais tarde.");
                    return View();
                }
            }
        
            ModelState.AddModelError(string.Empty, "Credenciais inválidas ou conta não existe.");

        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public IActionResult RecuperarSenha()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RecuperarSenha(RecuperarSenhaDto modelo)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(modelo.Email);
            if (user != null)
            {
                // Lógica para gerar token e enviar e-mail de recuperação de senha
                var tokenBruto = await _userManager.GeneratePasswordResetTokenAsync(user);
                var tokenCodificado = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(tokenBruto));

                var link = Url.Action("RecuperarSenha", "Account", new { userId = user.Id, token = tokenCodificado }, Request.Scheme);
                
                await _emailService.EnviarEmail(
                    user.Email,
                    "Recuperação de Senha",
                    $"Por favor, use o seguinte link para redefinir sua senha: <a href='{link}'>Redefinir Senha</a>"
                );

                ModelState.AddModelError(string.Empty, "Se o e-mail existir em nosso sistema, um link de recuperação foi enviado.");
            }
        }

        return View();
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorDto { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult SimularErro()
    {
        throw new Exception("Erro simulado para teste do manipulador de exceções.");
    }

    private async Task EnviarLinkDeConfirmacao(UsuarioModel user, string assunto = "Confirmação de E-mail")
    {
        // 1. Gera o token Base64 (com caracteres não seguros)
        var tokenBruto = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        // 2. CODIFICAÇÃO: Converte o token bruto para bytes e depois para Base64 URL-seguro.
        var tokenCodificado = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(tokenBruto));

        // 3. Usa o token CODIFICADO na URL
        var link = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = tokenCodificado }, Request.Scheme);

        var corpoEmail = $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <h2>Confirme seu endereço de e-mail</h2>
            <p>Olá {user.Nome},</p>
            <p>Obrigado por se cadastrar. Por favor, clique no botão abaixo para confirmar seu e-mail e ativar sua conta.</p>
            <p style='text-align: center;'>
                <a href='{link}' style='background-color: #007bff; color: white; padding: 14px 25px; text-align: center; text-decoration: none; display: inline-block; border-radius: 5px; font-size: 16px;'>
                Confirmar E-mail
                </a>
            </p>
            <p>Se o botão acima não funcionar, copie e cole o seguinte link no seu navegador:</p>
            <p><a href='{link}'>{link}</a></p>
            <hr>
            <p><small>Se você não criou esta conta, por favor, ignore este e-mail.</small></p>
            </div>";

        await _emailService.EnviarEmail(
            user.Email,
            assunto,
            corpoEmail
        );
    }
}   
