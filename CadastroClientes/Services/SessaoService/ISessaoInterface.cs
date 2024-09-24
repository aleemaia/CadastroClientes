using CadastroClientes.Models;

namespace CadastroClientes.Services.SessaoService
{
    public interface ISessaoInterface
    {
        UsuarioModel BuscarSessao();
        void CriarSessao(UsuarioModel usuarioModel);
        void RemoverSessao();
    }
}
