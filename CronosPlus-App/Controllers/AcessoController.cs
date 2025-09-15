using Acesso.Dtos;
using Acesso.Helpers;
using CronosPlus_App.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CronosPlus_App.Controllers
{
    public class AcessoController : Controller
    {
        private readonly IAcessoServiceApp _acessoService;
        private readonly IHttpContextAccessor _accessor;

        public AcessoController(IAcessoServiceApp acessoService,
                                IHttpContextAccessor accessor)
        {
            _acessoService = acessoService;
            _accessor = accessor;
        }

        public IActionResult Index()
        {
            if (UsuarioConectadoHelper.GetId(_accessor.HttpContext.User.Identity as ClaimsIdentity) > 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> Autenticar(LoginDto loginDto)
        {
            UsuarioAutenticadoDto Autentic = new();
            
            try
            {
                Autentic = await _acessoService.Login(loginDto);
            }
            catch (Exception)
            {
                TempData["erro"] = "Não foi possível efetuar a Autenticação.";
                return RedirectToAction("Index");
            }

            if (!Autentic.Autenticado)
            {
                TempData["erro"] = Autentic.Mensagem;
                return RedirectToAction("Index");
            }            

            ClaimsPrincipal identidade = UsuarioConectadoHelper.DefinirIdentidadeUsuario(Autentic);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identidade),
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddHours(4),
                    IsPersistent = true
                });

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Acesso", new { logout = true });
        }
    }
}
