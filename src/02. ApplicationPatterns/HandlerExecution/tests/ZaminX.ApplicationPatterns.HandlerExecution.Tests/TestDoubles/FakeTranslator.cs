using System.Globalization;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;

public sealed class FakeTranslator : ITranslator
{
    public string this[string key] => key;

    public string this[CultureInfo culture, string key] => key;

    public string this[string key, params string[] arguments] => Build(key, arguments);

    public string this[CultureInfo culture, string key, params string[] arguments] => Build(key, arguments);

    public string this[char separator, params string[] keys] => string.Join(separator, keys);

    public string this[CultureInfo culture, char separator, params string[] keys] => string.Join(separator, keys);

    public string GetString(string key) => key;

    public string GetString(CultureInfo culture, string key) => key;

    public string GetString(string key, params string[] arguments) => Build(key, arguments);

    public string GetString(CultureInfo culture, string key, params string[] arguments) => Build(key, arguments);

    public string GetFormattedString(string key, params object[] rawArguments)
    {
        return Build(key, rawArguments.Select(arg => arg?.ToString() ?? string.Empty));
    }

    public string GetFormattedString(CultureInfo culture, string key, params object[] rawArguments)
    {
        return Build(key, rawArguments.Select(arg => arg?.ToString() ?? string.Empty));
    }

    public string GetConcatString(char separator = ' ', params string[] keys) => string.Join(separator, keys);

    public string GetConcatString(CultureInfo culture, char separator = ' ', params string[] keys) => string.Join(separator, keys);

    private static string Build(string key, IEnumerable<string> arguments)
    {
        var args = arguments.ToArray();

        return args.Length == 0
            ? key
            : $"{key}:{string.Join(",", args)}";
    }
}