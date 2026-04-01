namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Models;

public sealed record TranslationEntry(
    string Key,
    string? Culture,
    string Value);
