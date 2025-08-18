using DataAccess.Context;
using DataAccess.Redis;
using DataAccess.Repository.Crud;
using DataAccess.Repository.Interfaces;
using Domain.ModelResponse;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class UsuarioRepository : CrudRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApiContext dbContext, IRedisCacheService cache) : base(dbContext, cache)
        {
        }

        public async Task<OperationResponse<List<Usuario>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder)
        {
            var response = new OperationResponse<List<Usuario>>();
            string cacheKey = $"cache_Usuario_Consultar_{limit.ToString()}_{offset.ToString()}_{search}_{colOrder.ToString()}_{dirOrder}";

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var cachedData = await _cache.GetAsync<Usuario>(cacheKey);
                    if (cachedData != null)
                    {
                        response.Success = true;
                        response.Object = cachedData;
                        response.Message = "Operação realizada com sucesso (cache).";

                        return response;
                    }
                }

                var query = _dbContext.Set<Usuario>()
                    .Where(x => !x.Deletado &&
                        (string.IsNullOrEmpty(search) || (
                           x.Id.ToString().Contains(search) ||
                           x.Nome!.Contains(search) ||
                           x.Email!.Contains(search) ||
                           x.NivelAcesso.ToString()!.Contains(search)
                        )))
                    .AsNoTracking();

                switch (colOrder)
                {
                    case 0:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case 1:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Nome) : query.OrderByDescending(x => x.Nome);
                        break;
                    case 2:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email);
                        break;
                    case 3:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.NivelAcesso) : query.OrderByDescending(x => x.NivelAcesso);
                        break;
                    default:
                        query = query.OrderBy(x => x.Nome);
                        break;
                }

                var lista = await query
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync();

                response.Success = true;
                response.Object = lista;
                response.Message = "Operação realizada com sucesso.";

                if (!string.IsNullOrEmpty(search))
                {
                    await _cache.SetAsync(cacheKey, lista, TimeSpan.FromMinutes(2));
                }

                return response;
            }
            catch (Exception e)
            {
                ClearDbContextState(_dbContext);

                response.Success = false;
                response.Object = new List<Usuario>();
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";

                return response;
            }
        }
    }
}
