using Acesso.Dtos;

namespace Intelectah_App.Services.Interfaces
{
    public interface IAcessoServiceApp
    {
        Task<UsuarioAutenticadoDto> Login(LoginDto loginDto);
    }
}
