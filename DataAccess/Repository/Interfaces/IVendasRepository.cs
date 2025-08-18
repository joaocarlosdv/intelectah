using DataAccess.Repository.Crud;
using Domain.ModelResponse;
using Domain.Models;

namespace DataAccess.Repository.Interfaces
{
    public interface IVendasRepository : ICrudRepository<Vendas>
    {
        Task<OperationResponse<List<Vendas>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
    }
}
