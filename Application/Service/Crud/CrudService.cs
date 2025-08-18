using Application.Dtos;
using Application.Dtos.Crud;
using AutoMapper;
using DataAccess.Repository.Crud;
using Domain.Models.Crud;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace Application.Service.Crud
{
    public class CrudService<T, Dto> : ICrudService<T, Dto> where T : ModelCrud where Dto : DtoCrud
    {
        public readonly IMapper _mapper;
        public readonly ICrudRepository<T> _repositorio;
        public readonly IHttpContextAccessor _accessor;

        public CrudService(IMapper mapper, ICrudRepository<T> repositorio, IHttpContextAccessor accessor)
        {
            _mapper = mapper;
            _repositorio = repositorio;
            _accessor = accessor;
        }
        public virtual async Task<OperationResponseDto<List<Dto>>> Consultar()
        {
            return _mapper.Map<OperationResponseDto<List<Dto>>>(await _repositorio.Consultar());
        }
        public virtual async Task<OperationResponseDto<List<Dto>>> Consultar(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            return _mapper.Map<OperationResponseDto<List<Dto>>>(await _repositorio.Consultar(predicate, includes));
        }
        public virtual async Task<OperationResponseDto<Dto>> Salvar(Dto dto)
        {
            if (dto.Id == 0)
            {
                return _mapper.Map<OperationResponseDto<Dto>>(await _repositorio.Inserir(_mapper.Map<T>(dto)));
            }
            else
            {
                return _mapper.Map<OperationResponseDto<Dto>>(await _repositorio.Alterar(_mapper.Map<T>(dto)));
            }
        }
        public virtual async Task<OperationResponseDto<Dto>> Deletar(int id)
        {
            var dto = (await _repositorio.Consultar(x => x.Id == id, null)).Object!.FirstOrDefault();
            return _mapper.Map<OperationResponseDto<Dto>>(await _repositorio.Deletar(_mapper.Map<T>(dto)));
        }
    }
}
