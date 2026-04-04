namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;

public interface ITranslationRefreshService
{
    Task RefreshAsync(CancellationToken cancellationToken = default);
}