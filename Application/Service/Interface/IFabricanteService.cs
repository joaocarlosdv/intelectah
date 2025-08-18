using Application.Dtos;
using Application.Service.Crud;
using Domain.Models;

namespace Application.Service.Interface
{
    public interface IFabricanteService : ICrudService<Fabricante, FabricanteDto>
    {
        Task<OperationResponseDto<List<FabricanteDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
        Task<OperationResponseDto<int>> ConsultaPaginadaCount(string? search, int colOrder, string dirOrder);
    }
}
