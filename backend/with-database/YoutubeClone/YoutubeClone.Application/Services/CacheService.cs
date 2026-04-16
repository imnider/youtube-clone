using Microsoft.Extensions.Caching.Memory;
using YoutubeClone.Application.Interfaces.Services;

namespace YoutubeClone.Application.Services
{
    public class CacheService(IMemoryCache memoryCache) : ICacheService
    {
        public T Create<T>(string key, TimeSpan expiration, T value)
        {
            try
            {
                var create = memoryCache.GetOrCreate(key, (factory) =>
                {
                    factory.SlidingExpiration = expiration;
                    return value;
                });
                return create is null ? throw new Exception("No se pudo establecer la caché") : create;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Delete(string key)
        {
            try
            {
                memoryCache.Remove(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public T? Get<T>(string key)
        {
            try
            {
                return memoryCache.Get<T>(key);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
