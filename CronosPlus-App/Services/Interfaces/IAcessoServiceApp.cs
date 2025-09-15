using Acesso.Dtos;

namespace CronosPlus_App.Services.Interfaces
{
    public interface IAcessoServiceApp
    {
        Task<UsuarioAutenticadoDto> Login(LoginDto loginDto);
    }
}
