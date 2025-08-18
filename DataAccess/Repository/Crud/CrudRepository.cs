using DataAccess.Context;
using DataAccess.Redis;
using Domain.ModelResponse;
using Domain.Models.Crud;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DataAccess.Repository.Crud
{
    public class CrudRepository<T> : ICrudRepository<T> where T : ModelCrud
    {
        public readonly ApiContext _dbContext;
        protected DbSet<T> DbSet => _dbContext.Set<T>();
        public readonly IRedisCacheService _cache;
        public CrudRepository(ApiContext dbContext, IRedisCacheService cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        protected void ClearDbContextState(DbContext context)
        {
            var entries = context.ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }
        public virtual async Task BeginTransactionAsync()
        {
            if (_dbContext.Database.CurrentTransaction == null)
            {
                await _dbContext.Database.BeginTransactionAsync();
            }
        }
        public virtual async Task CommitTransactionAsync()
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await _dbContext.Database.CommitTransactionAsync();
            }
        }
        public virtual async Task RollbackTransactionAsync()
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await _dbContext.Database.RollbackTransactionAsync();
            }
        }

        public virtual async Task<OperationResponse<List<T>>> Consultar()
        {
            var response = new OperationResponse<List<T>>();
            string cacheKey = $"cache_{typeof(T).Name}_Consultar";

            try
            {
                var cachedData = await _cache.GetAsync<T>(cacheKey);
                if (cachedData != null)
                {
                    response.Success = true;
                    response.Object = cachedData;
                    response.Message = "Operação realizada com sucesso (cache).";
                    return response;
                }

                var dados = await _dbContext.Set<T>()
                    .Where(x => !x.Deletado)
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
                response.Object = new List<T>();
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";
            }

            return response;
        }
        public virtual async Task<OperationResponse<List<T>>> Consultar(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
        {
            var response = new OperationResponse<List<T>>();

            string cacheKey = $"cache_{typeof(T).Name}_Consultar_{ExpressionHashHelper.GetExpressionKey(predicate).GetHashCode()}_{string.Join("_", includes?.Select(i => i.Body.ToString()) ?? new List<string>())}";

            var query = _dbContext.Set<T>().AsQueryable();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, includes) => current.Include(includes));
            }

            try
            {
                var cachedData = await _cache.GetAsync<T>(cacheKey);
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
                response.Object = new List<T>();
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";
            }

            return response;
        }
        public virtual async Task<OperationResponse<T>> Inserir(T entity)
        {
            var response = new OperationResponse<T>();

            try
            {
                _dbContext.Set<T>().Add(entity);
                await _dbContext.SaveChangesAsync();

                response.Success = true;
                response.Object = entity;
                response.Message = "Operação realizada com sucesso.";
            }
            catch (Exception e)
            {
                ClearDbContextState(_dbContext);

                response.Success = false;
                response.Object = entity;
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";
            }

            return response;
        }
        public virtual async Task<OperationResponse<T>> Alterar(T entity)
        {
            var response = new OperationResponse<T>();

            try
            {
                var entry = _dbContext.Entry(entity);
                entry.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                response.Success = true;
                response.Object = entity;
                response.Message = "Operação realizada com sucesso.";
            }
            catch (Exception e)
            {
                ClearDbContextState(_dbContext);

                response.Success = false;
                response.Object = entity;
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";
            }

            return response;
        }
        public virtual async Task<OperationResponse<T>> Deletar(T entity)
        {
            var response = new OperationResponse<T>();
            entity.Deletado = true;

            try
            {
                var entry = _dbContext.Entry(entity);
                entry.State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                response.Success = true;
                response.Object = entity;
                response.Message = "Operação realizada com sucesso.";
            }
            catch (Exception e)
            {
                ClearDbContextState(_dbContext);

                response.Success = false;
                response.Object = entity;
                response.Exception = e;
                response.Message = "Erro ao realizar operação.";
            }

            return response;
        }
    }

    public static class ExpressionHashHelper
    {
        public static string GetExpressionKey<T>(Expression<Func<T, bool>> expression)
        {
            var evaluator = new ValueEvaluator();
            var evaluated = (LambdaExpression)evaluator.Visit(expression);

            return evaluated.ToString(); 
        }

        private class ValueEvaluator : ExpressionVisitor
        {
            protected override Expression VisitMember(MemberExpression node)
            {                
                if (node.Expression is ConstantExpression constant)
                {
                    object container = constant.Value;

                    switch (node.Member)
                    {
                        case FieldInfo field:
                            object fieldValue = field.GetValue(container);
                            return Expression.Constant(fieldValue, node.Type);

                        case PropertyInfo prop:
                            object propValue = prop.GetValue(container);
                            return Expression.Constant(propValue, node.Type);
                    }
                }

                return base.VisitMember(node);
            }
        }
    }
}
