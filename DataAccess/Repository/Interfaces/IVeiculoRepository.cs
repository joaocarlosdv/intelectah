using DataAccess.Repository.Crud;
using Domain.ModelResponse;
using Domain.Models;

namespace DataAccess.Repository.Interfaces
{
    public interface IVeiculoRepository : ICrudRepository<Veiculo>
    {
        Task<OperationResponse<List<Veiculo>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
    }
}
