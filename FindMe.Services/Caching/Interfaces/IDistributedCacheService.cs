namespace FindMe.Services.Caching.Interfaces;

public interface IDistributedCacheService
{
    public Task<T?> GetData<T>(string key);
    public Task SetData<T>(string key, T value, TimeSpan expTime);
    public Task RemoveData(string key);
}