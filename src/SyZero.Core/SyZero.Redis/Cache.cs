using FreeRedis;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyZero.Cache;
using SyZero.Serialization;

namespace SyZero.Redis
{
    public class Cache : ICache
    {
        private readonly RedisClient _cache;
        private readonly IJsonSerialize _jsonSerialize;

        public Cache(RedisClient cache,
            IJsonSerialize jsonSerialize)
        {
            this._cache = cache;
            this._jsonSerialize = jsonSerialize;
        }

        public bool Exist(string key)
        {
            return _cache.Exists(key);
        }

        public T Get<T>(string key)
        {
            var jsonStr = _cache.Get<string>(key);
            if (string.IsNullOrEmpty(jsonStr)) { 
                return default(T);
            }
            return _jsonSerialize.JSONToObject<T>(jsonStr);
        }

        public string[] GetKeys(string pattern)
        {
           return _cache.Keys(pattern);
        }

        public void Refresh(string key)
        {
            _cache.EvalSha(key);
        }

        public async Task RefreshAsync(string key)
        {
            await Task.Run(() => { _cache.EvalSha(key); });
        }

        public void Remove(string key)
        {
            _cache.Del(key);//如果键值在Redis中不存在，IDistributedCache.Remove方法不会报错，但是如果传入的参数key为null，则会抛出异常
        }

        public async Task RemoveAsync(string key)
        {
            await Task.Run(() => { _cache.Del(key); });
        }

        public void Set<T>(string key, T value, int exprireTime = 24 * 60 * 60)
        {
            _cache.Set<string>(key, _jsonSerialize.ObjectToJSON(value), exprireTime);//将字节数组存入Redis
        }

        public async Task SetAsync<T>(string key, T value, int exprireTime = 24 * 60 * 60)
        {
            await Task.Run(() => { _cache.Set<string>(key, _jsonSerialize.ObjectToJSON(value), exprireTime); });
        }
    }
}
