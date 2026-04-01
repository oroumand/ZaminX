using System.Globalization;


namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;


public interface ITranslator
{
    string this[string key] { get; }
    string this[CultureInfo culture, string key] { get; }

    string this[string key, params string[] arguments] { get; }
    string this[CultureInfo culture, string key, params string[] arguments] { get; }

    string this[char separator, params string[] keys] { get; }
    string this[CultureInfo culture, char separator, params string[] keys] { get; }

    string GetString(string key);
    string GetString(CultureInfo culture, string key);

    string GetString(string key, params string[] arguments);
    string GetString(CultureInfo culture, string key, params string[] arguments);

    string GetFormattedString(string key, params object[] rawArguments);
    string GetFormattedString(CultureInfo culture, string key, params object[] rawArguments);

    string GetConcatString(char separator = ' ', params string[] keys);
    string GetConcatString(CultureInfo culture, char separator = ' ', params string[] keys);
}
