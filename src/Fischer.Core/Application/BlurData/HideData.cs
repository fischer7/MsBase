using Fischer.Core.Application.Attributes;

namespace Fischer.Core.Application.BlurData;
internal sealed class HideData
{
    internal static bool HasSensitiveData(Type type)
    {
        return type.GetProperties().Where(p => p.PropertyType == typeof(string) || p.PropertyType == typeof(int))
            .Select(k => k.GetCustomAttributes(true))
            .Any(x =>
                x.Any(i => i.GetType().Equals(typeof(SensitiveDataAttribute)))
                );
    }

    internal static void HideSensitiveData<TRequest>(TRequest request)
    {
        foreach (var prop in typeof(TRequest).GetProperties().Where(p => p.PropertyType == typeof(string)))
        {
            var attrs = prop.GetCustomAttributes(true);

            if (attrs.Any(x => x as SensitiveDataAttribute != null))
                prop.SetValue(request, "***blured***");
        }
        foreach (var prop in typeof(TRequest).GetProperties().Where(p => p.PropertyType == typeof(int)))
        {
            var attrs = prop.GetCustomAttributes(true);

            if (attrs.Any(x => x as SensitiveDataAttribute != null))
                prop.SetValue(request, -1);
        }
    }
}