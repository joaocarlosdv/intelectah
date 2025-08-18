using DataAccess.Repository.Crud;
using Domain.ModelResponse;
using Domain.Models;

namespace DataAccess.Repository.Interfaces
{
    public interface IConcessionariaRepository : ICrudRepository<Concessionaria>
    {
        Task<OperationResponse<List<Concessionaria>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder);
    }
}
