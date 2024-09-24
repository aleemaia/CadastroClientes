using CadastroClientes.Data;
using CadastroClientes.Dto;
using CadastroClientes.Models;
using CadastroClientes.Services.SenhaService;
using CadastroClientes.Services.SessaoService;
using Microsoft.AspNetCore.ResponseCompression;

namespace CadastroClientes.Services.LoginService
{
    public class LoginService : ILoginInterface
    {
        private readonly CadastroClientesDbContext _context;
        private readonly ISenhaInterface _senhaInterface;
        private readonly ISessaoInterface _sessaoInterface;

        public LoginService(CadastroClientesDbContext context, ISenhaInterface senhaInterface, ISessaoInterface sessaoInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
            _sessaoInterface = sessaoInterface;
        }

        public async Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();

            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(x=> x.Email == usuarioLoginDto.Email);

                if (usuario == null)
                {
                    response.Mensagem = "Credenciais Inválidas!";
                    response.Status = false;
                    return response;
                }

                if (!_senhaInterface.VerificaSenha(usuarioLoginDto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    response.Mensagem = "Credenciais Inválidas!";
                    response.Status = false;
                    return response;
                }

                _sessaoInterface.CriarSessao(usuario);

                response.Mensagem = "Usuário logado com sucesso!";
                return response;
            }
            catch (Exception ex)
            {

                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioRegisterDto usuarioRegisterDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();

            try
            {
                if (VerificarEmail(usuarioRegisterDto))
                {
                    response.Mensagem = "Email já Cadastrado!";
                    response.Status = false;
                    return response;
                }

                _senhaInterface.CriarSenhaHash(usuarioRegisterDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                var usuario = new UsuarioModel()
                {
                    Nome = usuarioRegisterDto.Nome,
                    Sobrenome = usuarioRegisterDto.Sobrenome,
                    Email = usuarioRegisterDto.Email,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
                };

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                response.Mensagem = "Usuário Cadastrado com Sucesso!!";

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        private bool VerificarEmail(UsuarioRegisterDto usuarioRegisterDto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == usuarioRegisterDto.Email);

            if (usuario == null) return false;
            return true;
        }
    }
}
