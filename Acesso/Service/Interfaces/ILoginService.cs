using Acesso.Dtos;

namespace Acesso.Service.Interfaces
{
    public interface ILoginService
    {
        Task<UsuarioAutenticadoDto> LoginAsync(LoginDto login);
    }
}
