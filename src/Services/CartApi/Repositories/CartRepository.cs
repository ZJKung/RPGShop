using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartApi.Domain;
using CartApi.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace CartApi.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ILogger<CartRepository> _logger;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public CartRepository(ILogger<CartRepository> logger, ConnectionMultiplexer redis)
        {
            _logger = logger;
            _redis = redis;
            _database = redis.GetDatabase();
        }
        private IServer GetServers()
        {
            var endpoint = _redis.GetEndPoints();
            return _redis.GetServer(endpoint.First());
        }
        public async Task<bool> DeleteAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public Task<IEnumerable<Cart>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Cart> GetAsync(string id)
        {
            var data = await _database.StringGetAsync(id);
            if (data.IsNullOrEmpty) return null;
            return JsonConvert.DeserializeObject<Cart>(data);
        }

        public async Task<Cart> UpdateAsync(Cart entity)
        {
            var created = await _database.StringSetAsync(entity.BuyerId, JsonConvert.SerializeObject(entity));
            if (!created)
            {
                _logger.LogInformation("Problem occur persisting the item");
                return null;
            }

            return await GetAsync(entity.BuyerId);
        }

        public IEnumerable<string> GetUsers()
        {
            var server = GetServers();
            var data = server.Keys();
            return data?.Select(k => k.ToString());
        }
    }
}