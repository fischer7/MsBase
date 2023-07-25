using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Fischer.Core.Extensions;
public static class DistributedCacheExtensions
{
    public static async Task<List<T>?> GetList<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken)
    {
        var dataStr = await cache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(dataStr)) return new List<T>();

        return JsonConvert.DeserializeObject<List<T>>(dataStr);
    }

    public static async Task SaveList<T>(
        this IDistributedCache cache,
        string key,
        List<T> list,
        int expirationTimeSpan = 2, 
        bool isMinutes = false,
        CancellationToken cancellationToken = default)
    {
        var serializedList = JsonConvert.SerializeObject(list);

        if (expirationTimeSpan <= 0) expirationTimeSpan = 2;

        await cache.SetStringAsync(key, serializedList, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = isMinutes ? TimeSpan.FromMinutes(expirationTimeSpan) : TimeSpan.FromHours(expirationTimeSpan)
        }, cancellationToken);
    }

    public static async Task Save(
        this IDistributedCache cache,
        string key,
        string value,
        DateTime validTo, 
        CancellationToken cancellationToken = default)
    {
        await cache.SetStringAsync(key, value, new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = new DateTimeOffset(validTo)
        }, cancellationToken);
    }


    public static async Task<T> SaveObject<T>(this IDistributedCache cache, string key, T value, TimeSpan? expiresIn = null)
        where T : class
    {
        var serializedValue = JsonConvert.SerializeObject(value);

        await cache.SetStringAsync(key, serializedValue, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiresIn
        });

        return value;
    }

    public static async Task<T?> GetObject<T>(this IDistributedCache cache, string key)
        where T : class
    {
        var result = await cache.GetStringAsync(key);

        if (result == null) return default;

        return JsonConvert.DeserializeObject<T>(result);
    }
}
