using DataAccess.Repository.Crud;
using Domain.ModelResponse;
using Domain.Models;

namespace DataAccess.Repository.Interfaces
{
    public interface IUsuarioRepository : ICrudRepository<Usuario>
    {
        Task<OperationResponse<List<Usuario>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
    }
}
