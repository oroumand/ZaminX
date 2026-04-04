namespace ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer.Configurations;

public sealed record ParrotSqlServerSeedTranslation(
    string Key,
    string? Culture,
    string Value);
