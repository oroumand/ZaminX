namespace ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer.Configurations;

public sealed class ParrotSqlServerOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public string Schema { get; set; } = "dbo";

    public string TableName { get; set; } = "ParrotTranslations";

    public int? CommandTimeoutSeconds { get; set; }

    public bool EnsureTableCreated { get; set; }

    public TranslationReloadMode ReloadMode { get; set; } = TranslationReloadMode.None;

    public TimeSpan? ReloadInterval { get; set; }


    public bool RegisterMissingKeys { get; set; }

    public string MissingKeyDefaultValueTemplate { get; set; } = "{0}";

    public ICollection<ParrotSqlServerSeedTranslation> SeedTranslations { get; set; } =
        new List<ParrotSqlServerSeedTranslation>();
}