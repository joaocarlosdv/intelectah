using Application.Dtos;
using Application.Dtos.Crud;
using Domain.Models.Crud;
using System.Linq.Expressions;

namespace Application.Service.Crud
{
    public interface ICrudService<T, Dto> where T : ModelCrud where Dto : DtoCrud
    {
        Task<OperationResponseDto<List<Dto>>> Consultar();
        Task<OperationResponseDto<List<Dto>>> Consultar(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes);
        Task<OperationResponseDto<Dto>> Salvar(Dto dto);
        Task<OperationResponseDto<Dto>> Deletar(int id);
    }
}
