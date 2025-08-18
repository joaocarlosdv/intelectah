using Application.Dtos;
using Application.Service.Crud;
using Domain.Models;

namespace Application.Service.Interface
{
    public interface IConcessionariaService : ICrudService<Concessionaria, ConcessionariaDto>
    {
        Task<OperationResponseDto<List<ConcessionariaDto>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
    }
}
