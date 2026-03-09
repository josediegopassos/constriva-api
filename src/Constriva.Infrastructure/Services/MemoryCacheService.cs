using Constriva.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constriva.Infrastructure.Services
{
    public class MemoryCacheService : ICacheService
    {
        private static readonly Dictionary<string, (object Value, DateTime Expiry)> _store = new();

        public Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        {
            if (_store.TryGetValue(key, out var item) && item.Expiry > DateTime.UtcNow)
                return Task.FromResult((T?)item.Value);
            return Task.FromResult(default(T));
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default)
        {
            _store[key] = (value!, DateTime.UtcNow.Add(expiry ?? TimeSpan.FromMinutes(30)));
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, CancellationToken ct = default) { _store.Remove(key); return Task.CompletedTask; }
        public Task RemoveByPatternAsync(string pattern, CancellationToken ct = default) => Task.CompletedTask;
    }
}
