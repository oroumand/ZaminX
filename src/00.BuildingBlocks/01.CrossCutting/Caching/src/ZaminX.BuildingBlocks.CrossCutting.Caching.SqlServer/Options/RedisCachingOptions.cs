namespace ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer.Options;

public sealed class SqlServerCachingOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public string SchemaName { get; set; } = "dbo";

    public string TableName { get; set; } = "StashXCache";

    public bool EnsureStorageOnStartup { get; set; }
}