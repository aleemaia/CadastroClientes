using CadastroClientes.Dto;
using CadastroClientes.Services.LoginService;
using CadastroClientes.Services.SessaoService;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClientes.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginInterface _loginInterface;
        private readonly ISessaoInterface _sessaoInterface;

        public LoginController(ILoginInterface loginInterface, ISessaoInterface sessaoInterface)
        {
            _loginInterface = loginInterface;
            _sessaoInterface =sessaoInterface;  
        }

        public IActionResult Login()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario != null) return RedirectToAction("Login", "Login");

            return View();
        }

        public IActionResult Logout()
        {
            _sessaoInterface.RemoverSessao();
            return RedirectToAction("Login");
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioRegisterDto usuarioRegisterDto)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _loginInterface.RegistrarUsuario(usuarioRegisterDto);

                if (usuario.Status)
                {
                    TempData["MensagemSucesso"] = usuario.Mensagem;
                    return RedirectToAction("Index");
                }

                TempData["MensagemErro"] = usuario.Mensagem;
                return View(usuarioRegisterDto);
            }

            return View(usuarioRegisterDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLoginDto)
        {
            if (!ModelState.IsValid) return View(usuarioLoginDto);

            var usuario = await _loginInterface.Login(usuarioLoginDto);

            if (usuario.Status)
            {
                TempData["MensagemSucesso"] = usuario.Mensagem;
                return RedirectToAction("Index", "Home");
            }

            TempData["MensagemErro"] = usuario.Mensagem;
            return View(usuarioLoginDto);
        }
    }
}
