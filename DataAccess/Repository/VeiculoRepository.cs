using DataAccess.Context;
using DataAccess.Redis;
using DataAccess.Repository.Crud;
using DataAccess.Repository.Interfaces;
using Domain.ModelResponse;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class VeiculoRepository : CrudRepository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(ApiContext dbContext, IRedisCacheService cache) : base(dbContext, cache)
        {
        }

        public async Task<OperationResponse<List<Veiculo>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder)
        {
            var response = new OperationResponse<List<Veiculo>>();
            string cacheKey = $"cache_Veiculo_Consultar_{limit.ToString()}_{offset.ToString()}_{search}_{colOrder.ToString()}_{dirOrder}";

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var cachedData = await _cache.GetAsync<Veiculo>(cacheKey);
                    if (cachedData != null)
                    {
                        response.Success = true;
                        response.Object = cachedData;
                        response.Message = "Operação realizada com sucesso (cache).";

                        return response;
                    }
                }

                var query = _dbContext.Set<Veiculo>()
                    .Include(x => x.Fabricante)
                    .Where(x => !x.Deletado &&
                        (string.IsNullOrEmpty(search) || (
                           x.Id.ToString().Contains(search) ||
                           x.Modelo!.Contains(search) ||
                           x.Fabricante!.Nome!.Contains(search) ||
                           x.TipoVeiculo.ToString().Contains(search) ||
                           x.AnoFabricacao.ToString().Contains(search) ||
                           x.Preco.ToString().Contains(search)
                        )))
                    .AsNoTracking();

                switch (colOrder)
                {
                    case 0:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case 1:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Modelo) : query.OrderByDescending(x => x.Modelo);
                        break;
                    case 2:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Fabricante!.Nome) : query.OrderByDescending(x => x.Fabricante!.Nome);
                        break;
                    case 3:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.TipoVeiculo) : query.OrderByDescending(x => x.TipoVeiculo);
                        break;
                    case 4:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.AnoFabricacao) : query.OrderByDescending(x => x.AnoFabricacao);
                        break;
                    case 5:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Preco) : query.OrderByDescending(x => x.Preco);
                        break;
                    default:
                        query = query.OrderBy(x => x.Modelo);
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
                response.Object = new List<Veiculo>();
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";

                return response;
            }
        }
    }
}
