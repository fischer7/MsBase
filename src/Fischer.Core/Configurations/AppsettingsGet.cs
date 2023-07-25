using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Fischer.Core.Configurations;
public sealed class AppsettingsGet
{
    private readonly IConfiguration _configuration;
    private readonly Dictionary<string, string?> _parameters;
    private readonly string Name;

    public AppsettingsGet(IWebHostEnvironment environment, IConfiguration configuration)
    {
        Name = environment.EnvironmentName.Trim();
        _configuration = configuration;
        _parameters = configuration.AsEnumerable()
            .ToDictionary(x => x.Key, x => x.Value);
    }

    public string GetName() => Name;
    public string? this[string key] => _parameters.ContainsKey(key) ? _parameters[key] : default;
    public T? GetConfig<T>() where T : class => _configuration.GetSection(typeof(T).Name).Get<T>();
    public T? GetConfig<T>(string section) where T : class => _configuration.GetSection(section).Get<T>();
    public bool IsDevelopment() => string.Equals(Name, Environments.Development, StringComparison.CurrentCultureIgnoreCase);
}