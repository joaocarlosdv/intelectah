using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DataAccess.Redis
{
    public static class RedisOptions
    {
        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
        }
    }
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _cache;
        public RedisCacheService(IConfiguration configuration)
        {
            var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!);
            _cache = redis.GetDatabase();
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var json = JsonConvert.SerializeObject(value);
            await _cache.StringSetAsync(key, json, expiration);
        }
        //public async Task<T?> GetAsync<T>(string key)
        //{
        //    var value = await _cache.StringGetAsync(key);
        //    if (value.IsNullOrEmpty) return default;

        //    var jsonString = value.ToString();
        //    var retorno = JsonConvert.DeserializeObject<T>(jsonString);

        //    return retorno;
        //}
        public async Task<List<T>?> GetAsync<T>(string key)
        {
            var value = await _cache.StringGetAsync(key);
            if (value.IsNullOrEmpty) return null;

            return JsonConvert.DeserializeObject<List<T>>(value!);
        }
    }
}
