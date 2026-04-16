namespace YoutubeClone.Application.Interfaces.Services
{
    public interface ICacheService
    {
        public T Create<T>(string key, TimeSpan expiration, T value);
        public T? Get<T>(string key);
        public bool Delete(string key);
    }
}
