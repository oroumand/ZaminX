using System.Globalization;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Services;

internal sealed class ParrotTranslator(
    ParrotTranslationStore store,
    ITranslationMissingKeyRegistrar missingKeyRegistrar) : ITranslator
{
    public string this[string key] => GetString(key);

    public string this[CultureInfo culture, string key] => GetString(culture, key);

    public string this[string key, params string[] arguments] => GetString(key, arguments);

    public string this[CultureInfo culture, string key, params string[] arguments] => GetString(culture, key, arguments);

    public string this[char separator, params string[] keys] => GetConcatString(separator, keys);

    public string this[CultureInfo culture, char separator, params string[] keys] => GetConcatString(culture, separator, keys);

    public string GetString(string key)
    {
        return GetString(CultureInfo.CurrentCulture, key);
    }

    public string GetString(CultureInfo culture, string key)
    {
        ArgumentNullException.ThrowIfNull(culture);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        if (store.TryGetValue(culture, key, out var translatedValue) && !string.IsNullOrWhiteSpace(translatedValue))
        {
            return translatedValue;
        }

        _ = missingKeyRegistrar.RegisterIfNeededAsync(key, culture);

        return key;
    }

    public string GetString(string key, params string[] arguments)
    {
        return GetString(CultureInfo.CurrentCulture, key, arguments);
    }

    public string GetString(CultureInfo culture, string key, params string[] arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);

        var format = GetString(culture, key);
        var translatedArguments = arguments
            .Select(argument => GetString(culture, argument))
            .Cast<object?>()
            .ToArray();

        return string.Format(culture, format, translatedArguments);
    }

    public string GetFormattedString(string key, params object[] rawArguments)
    {
        return GetFormattedString(CultureInfo.CurrentCulture, key, rawArguments);
    }

    public string GetFormattedString(CultureInfo culture, string key, params object[] rawArguments)
    {
        ArgumentNullException.ThrowIfNull(culture);
        ArgumentNullException.ThrowIfNull(rawArguments);

        var format = GetString(culture, key);
        return string.Format(culture, format, rawArguments);
    }

    public string GetConcatString(char separator = ' ', params string[] keys)
    {
        return GetConcatString(CultureInfo.CurrentCulture, separator, keys);
    }

    public string GetConcatString(CultureInfo culture, char separator = ' ', params string[] keys)
    {
        ArgumentNullException.ThrowIfNull(culture);
        ArgumentNullException.ThrowIfNull(keys);

        var translatedValues = keys.Select(key => GetString(culture, key));
        return string.Join(separator, translatedValues);
    }
}