using Application.Dtos;
using Application.Service.Crud;
using Domain.Models;

namespace Application.Service.Interface
{
    public interface IVeiculoService : ICrudService<Veiculo, VeiculoDto>
    {
        Task<OperationResponseDto<List<VeiculoDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
    }
}
