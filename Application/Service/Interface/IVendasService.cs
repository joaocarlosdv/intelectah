using Application.Dtos;
using Application.Service.Crud;
using Domain.Models;

namespace Application.Service.Interface
{
    public interface IVendasService : ICrudService<Vendas, VendasDto>
    {
        Task<OperationResponseDto<List<VendasDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
    }
}
