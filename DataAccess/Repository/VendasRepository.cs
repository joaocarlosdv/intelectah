using DataAccess.Context;
using DataAccess.Redis;
using DataAccess.Repository.Crud;
using DataAccess.Repository.Interfaces;
using Domain.ModelResponse;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public class VendasRepository : CrudRepository<Vendas>, IVendasRepository
    {
        public VendasRepository(ApiContext dbContext, IRedisCacheService cache) : base(dbContext, cache)
        {
        }

        public async Task<OperationResponse<List<Vendas>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder)
        {
            var response = new OperationResponse<List<Vendas>>();
            string cacheKey = $"cache_Cliente_Consultar_{limit.ToString()}_{offset.ToString()}_{search}_{colOrder.ToString()}_{dirOrder}";

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var cachedData = await _cache.GetAsync<Vendas>(cacheKey);
                    if (cachedData != null)
                    {
                        response.Success = true;
                        response.Object = cachedData;
                        response.Message = "Operação realizada com sucesso (cache).";

                        return response;
                    }
                }

                var query = _dbContext.Set<Vendas>()
                    .Include(x => x.Cliente)
                    .Include(x => x.Veiculo)
                    .Include(x => x.Concessionaria)
                    .Where(x => !x.Deletado &&
                        (string.IsNullOrEmpty(search) || (
                           x.Id.ToString().Contains(search) ||
                           x.Veiculo!.Modelo!.Contains(search) ||
                           x.Concessionaria!.Nome!.Contains(search) ||
                           x.Cliente!.Nome!.Contains(search) ||
                           x.ProtocoloVenda!.Contains(search) ||
                           x.DataVenda.ToString().Contains(search) ||
                           x.PrecoVenda.ToString().Contains(search)
                        )))
                    .AsNoTracking();

                switch (colOrder)
                {
                    case 0:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case 1:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.ProtocoloVenda) : query.OrderByDescending(x => x.ProtocoloVenda);
                        break;
                    case 2:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Cliente!.Nome) : query.OrderByDescending(x => x.Cliente!.Nome);
                        break;
                    case 3:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Veiculo!.Modelo) : query.OrderByDescending(x => x.Veiculo!.Descricao);
                        break;
                    case 4:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.Concessionaria!.Nome) : query.OrderByDescending(x => x.Concessionaria!.Nome);
                        break;
                    case 5:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.DataVenda) : query.OrderByDescending(x => x.DataVenda);
                        break;
                    case 6:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.PrecoVenda) : query.OrderByDescending(x => x.PrecoVenda);
                        break;
                    default:
                        query = query.OrderBy(x => x.ProtocoloVenda);
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
                response.Object = new List<Vendas>();
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";

                return response;
            }
        }

        public override async Task<OperationResponse<List<Vendas>>> Consultar(Expression<Func<Vendas, bool>> predicate, List<Expression<Func<Vendas, object>>> includes)
        {
            var response = new OperationResponse<List<Vendas>>();

            string cacheKey = $"cache_{typeof(Vendas).Name}_Consultar_{ExpressionHashHelper.GetExpressionKey(predicate).GetHashCode()}";

            var query = _dbContext.Set<Vendas>()
                .Include(x => x.Cliente)
                .Include(x => x.Concessionaria)
                .Include(x => x.Veiculo)
                .Include(x => x.Veiculo).ThenInclude(y => y.Fabricante)
                .AsQueryable();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, includes) => current.Include(includes));
            }

            try
            {
                var cachedData = await _cache.GetAsync<Vendas>(cacheKey);
                if (cachedData != null)
                {
                    response.Success = true;
                    response.Object = cachedData;
                    response.Message = "Operação realizada com sucesso (cache).";
                    return response;
                }

                var dados = await query
                    .Where(predicate)
                    .AsNoTracking()
                    .ToListAsync();

                response.Success = true;
                response.Object = dados;
                response.Message = "Operação realizada com sucesso.";

                await _cache.SetAsync(cacheKey, dados, TimeSpan.FromMinutes(2));
            }
            catch (Exception e)
            {
                ClearDbContextState(_dbContext);

                response.Success = false;
                response.Object = new List<Vendas>();
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";
            }

            return response;
        }
    }
}
