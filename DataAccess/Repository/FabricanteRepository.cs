using Azure;
using DataAccess.Context;
using DataAccess.Redis;
using DataAccess.Repository.Crud;
using DataAccess.Repository.Interfaces;
using Domain.ModelResponse;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;

namespace DataAccess.Repository
{
    public class FabricanteRepository : CrudRepository<Fabricante>, IFabricanteRepository
    {
        public FabricanteRepository(ApiContext dbContext, IRedisCacheService cache) : base(dbContext, cache)
        {
        }
        public async Task<OperationResponse<List<Fabricante>>> ConsultaPaginada(int limit, int offset, string? search, int colOrder, string dirOrder)
        {
            var response = new OperationResponse<List<Fabricante>>();
            string cacheKey = $"cache_Fabricante_Consultar_{limit.ToString()}_{offset.ToString()}_{search}_{colOrder.ToString()}_{dirOrder}";

            try
            {
                if (!string.IsNullOrEmpty(search))
                {                    
                    var cachedData = await _cache.GetAsync<Fabricante>(cacheKey);
                    if (cachedData != null)
                    {
                        response.Success = true;
                        response.Object = cachedData;
                        response.Message = "Operação realizada com sucesso (cache).";

                        return response;
                    }
                }                

                var query = _dbContext.Set<Fabricante>()
                    .Where(x => !x.Deletado && 
                        (string.IsNullOrEmpty(search) || (
                           x.Id.ToString().Contains(search) ||
                           x.Nome!.Contains(search) ||
                           x.PaisOrigem!.Contains(search) ||
                           x.AnoFundacao.ToString().Contains(search) ||
                           x.WebSite!.Contains(search)
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
                        query = dirOrder == "asc" ? query.OrderBy(x => x.PaisOrigem) : query.OrderByDescending(x => x.PaisOrigem);
                        break;
                    case 3:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.AnoFundacao) : query.OrderByDescending(x => x.AnoFundacao);
                        break;
                    case 4:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.WebSite) : query.OrderByDescending(x => x.WebSite);
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
                response.Object = new List<Fabricante>();
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";

                return response;
            }
        }
        public async Task<OperationResponse<int>> ConsultaPaginadaCount(string? search, int colOrder, string dirOrder)
        {
            var response = new OperationResponse<int>();
            string cacheKey = $"cache_Fabricante_Consultar_{search}_{colOrder.ToString()}_{dirOrder}";

            try
            {  
                var query = _dbContext.Set<Fabricante>()
                    .Where(x => !x.Deletado &&
                        (string.IsNullOrEmpty(search) || (
                           x.Id.ToString().Contains(search) ||
                           x.Nome!.Contains(search) ||
                           x.PaisOrigem!.Contains(search) ||
                           x.AnoFundacao.ToString().Contains(search) ||
                           x.WebSite!.Contains(search)
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
                        query = dirOrder == "asc" ? query.OrderBy(x => x.PaisOrigem) : query.OrderByDescending(x => x.PaisOrigem);
                        break;
                    case 3:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.AnoFundacao) : query.OrderByDescending(x => x.AnoFundacao);
                        break;
                    case 4:
                        query = dirOrder == "asc" ? query.OrderBy(x => x.WebSite) : query.OrderByDescending(x => x.WebSite);
                        break;
                    default:
                        query = query.OrderBy(x => x.Nome);
                        break;
                }

                var retorno = await query.CountAsync();                

                response.Success = true;
                response.Object = retorno;
                response.Message = "Operação realizada com sucesso.";

                return response;
            }
            catch (Exception e)
            {
                ClearDbContextState(_dbContext);

                response.Success = false;
                response.Object = 0;
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";

                return response;
            }
        }
    }
}
