using DataAccess.Repository.Crud;
using Domain.ModelResponse;
using Domain.Models;

namespace DataAccess.Repository.Interfaces
{
    public interface IFabricanteRepository : ICrudRepository<Fabricante>
    {
        Task<OperationResponse<List<Fabricante>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
        Task<OperationResponse<int>> ConsultaPaginadaCount(string? search, int colOrder, string dirOrder);
    }
}
