using DataAccess.Context;
using DataAccess.Redis;
using DataAccess.Repository.Crud;
using DataAccess.Repository.Interfaces;
using Domain.ModelResponse;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class ClienteRepository : CrudRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ApiContext dbContext, IRedisCacheService cache) : base(dbContext, cache)
        {
        }

        public async Task<OperationResponse<List<Cliente>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder)
        {
            var response = new OperationResponse<List<Cliente>>();
            string cacheKey = $"cache_Cliente_Consultar_{limit.ToString()}_{offset.ToString()}_{search}_{colOrder.ToString()}_{dirOrder}";

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var cachedData = await _cache.GetAsync<Cliente>(cacheKey);
                    if (cachedData != null)
                    {
                        response.Success = true;
                        response.Object = cachedData;
                        response.Message = "Operação realizada com sucesso (cache).";

                        return response;
                    }
                }

                var query = _dbContext.Set<Cliente>()
                    .Where(x => !x.Deletado &&
                        (string.IsNullOrEmpty(search) || (
                           x.Id.ToString().Contains(search) ||
                           x.Nome!.Contains(search) ||
                           x.Cpf!.Contains(search) ||
                           x.Telefone!.Contains(search)
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
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Cpf) : query.OrderByDescending(x => x.Cpf);
                        break;
                    case 3:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Telefone) : query.OrderByDescending(x => x.Telefone);
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
                response.Object = new List<Cliente>();
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";

                return response;
            }
        }
    }
}
