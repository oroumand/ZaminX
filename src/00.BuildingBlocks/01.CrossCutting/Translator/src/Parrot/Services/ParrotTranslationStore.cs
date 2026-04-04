using System.Globalization;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Models;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Services;

internal sealed class ParrotTranslationStore
{
    private readonly object _syncRoot = new();
    private Dictionary<string, string> _entries = new(StringComparer.OrdinalIgnoreCase);

    public void ReplaceAll(IEnumerable<TranslationEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var entry in entries)
        {
            if (string.IsNullOrWhiteSpace(entry.Key))
            {
                continue;
            }

            var compositeKey = BuildCompositeKey(entry.Culture, entry.Key);
            dictionary[compositeKey] = entry.Value;
        }

        lock (_syncRoot)
        {
            _entries = dictionary;
        }
    }

    public bool TryGetValue(CultureInfo? culture, string key, out string? value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        Dictionary<string, string> snapshot;

        lock (_syncRoot)
        {
            snapshot = _entries;
        }

        foreach (var cultureName in GetCultureFallbackChain(culture))
        {
            var compositeKey = BuildCompositeKey(cultureName, key);
            if (snapshot.TryGetValue(compositeKey, out value))
            {
                return true;
            }
        }

        value = null;
        return false;
    }

    private static IEnumerable<string?> GetCultureFallbackChain(CultureInfo? culture)
    {
        if (culture is not null)
        {
            var current = culture;

            while (true)
            {
                if (!string.IsNullOrWhiteSpace(current.Name))
                {
                    yield return current.Name;
                }

                if (current.Equals(CultureInfo.InvariantCulture))
                {
                    break;
                }

                current = current.Parent;
            }
        }

        yield return null;
    }

    private static string BuildCompositeKey(string? culture, string key)
    {
        var normalizedCulture = string.IsNullOrWhiteSpace(culture) ? "*" : culture.Trim();
        return $"{normalizedCulture}::{key.Trim()}";
    }
}