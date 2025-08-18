using Application.Dtos;
using Application.Service.Crud;
using Domain.Models;

namespace Application.Service.Interface
{
    public interface IUsuarioService : ICrudService<Usuario, UsuarioDto>
    {
        Task<OperationResponseDto<List<UsuarioDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
    }
}
