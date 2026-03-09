using Constriva.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Constriva.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        public RedisCacheService(IDistributedCache cache) => _cache = cache;

        public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        {
            var value = await _cache.GetStringAsync(key, ct);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default)
        {
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(30) };
            await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), options, ct);
        }

        public async Task RemoveAsync(string key, CancellationToken ct = default)
            => await _cache.RemoveAsync(key, ct);

        public Task RemoveByPatternAsync(string pattern, CancellationToken ct = default) => Task.CompletedTask;
    }
}
