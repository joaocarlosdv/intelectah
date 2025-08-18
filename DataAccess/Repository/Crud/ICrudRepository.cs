using Domain.ModelResponse;
using Domain.Models.Crud;
using System.Linq.Expressions;

namespace DataAccess.Repository.Crud
{
    public interface ICrudRepository<T> where T : ModelCrud
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<OperationResponse<List<T>>> Consultar();
        Task<OperationResponse<List<T>>> Consultar(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes);
        Task<OperationResponse<T>> Inserir(T entity);
        Task<OperationResponse<T>> Alterar(T entity);
        Task<OperationResponse<T>> Deletar(T entity);
    }
}
