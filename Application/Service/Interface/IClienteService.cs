using Application.Dtos;
using Application.Service.Crud;
using Domain.Models;

namespace Application.Service.Interface
{
    public interface IClienteService : ICrudService<Cliente, ClienteDto>
    {
        Task<OperationResponseDto<List<ClienteDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
    }
}
