using DataAccess.Repository.Crud;
using Domain.ModelResponse;
using Domain.Models;

namespace DataAccess.Repository.Interfaces
{
    public interface IClienteRepository : ICrudRepository<Cliente>
    {
        Task<OperationResponse<List<Cliente>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
    }
}
